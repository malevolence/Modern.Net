(function (window, $, undefined) {
    'use strict';

    var BW = window.BW = window.BW || {};

    BW.pushNotifications = {};

    BW.pushNotifications.subscribeUser = function () {
        return navigator.serviceWorker.register('/sw.js')
            .then(function (registration) {
                const subcribeOpts = {
                    userVisibleOnly: true,
                    applicationServerKey: urlBase64ToUint8Array('BOFHKvJfY8djKneLV_Volxhgl0yUwdQk-tur8yUYYsL_lIEiLtvaG2cGF98kAv1NKgTiN6C2E1aUehcSQbg5lAw')
                };
                return registration.pushManager.subscribe(subcribeOpts);
            }, function (err) {
                console.log('ServiceWorker registration failed: ', err);
            })
            .then(function (pushSubscription) {
                console.log('Received PushSubscription: ', JSON.stringify(pushSubscription));
                return pushSubscription;
            });
    };

    BW.pushNotifications.saveSubscriptionToServer = function (subscription) {
        console.log('subscription', subscription);

        return fetch('/api/notifications', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(subscription)
        })
        .then(function (response) {
            console.log('response', response);
            if (!response.ok) {
                throw new Error('Bad status code from the server');
            }
            return response.json();
        })
        .then(function (resData) {
            console.log('resData', resData);

            if (!(resData.data && resData.data.success)) {
                throw new Error('Bad response from the server');
            }
        });
    };

    BW.pushNotifications.buttonClickHandler = function (ev) {
        ev.preventDefault();
        BW.pushNotifications.subscribeUser().then(function (sub) {
            BW.pushNotifications.saveSubscriptionToServer(sub).then(function () {
                // don't allow them to subscribe again
                // remove click handler
                // disable button
                var btn = document.getElementById('btnSubscribe');
                if (btn) {
                    btn.removeEventListener('click', BW.pushNotifications.buttonClickHandler);
                    btn.disabled = true;
                }
            });
        });
    };

    BW.pushNotifications.init = function () {
        // setup the event handlers
        var btn = document.getElementById('btnSubscribe');
        if (btn) {
            btn.addEventListener('click', BW.pushNotifications.buttonClickHandler);
        }
    };

    function urlBase64ToUint8Array(base64String) {
        const padding = '='.repeat((4 - base64String.length % 4) % 4);
        const base64 = (base64String + padding)
            .replace(/\-/g, '+')
            .replace(/_/g, '/')
            ;
        const rawData = window.atob(base64);
        return Uint8Array.from([...rawData].map((char) => char.charCodeAt(0)));
    }

    document.addEventListener('DOMContentLoaded', function (ev) {
        BW.pushNotifications.init();
    });
})(window, jQuery);