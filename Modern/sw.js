(function () {
    'use strict';

    var CACHE_NAME = 'modern-net-cache-v4';
    var urlsToCache = [
        '/',
        '/content/site.css',
        '/js/site.js'
    ];

    self.addEventListener('install', function (ev) {
        ev.waitUntil(
            caches.open(CACHE_NAME)
                .then(function (cache) {
                    console.log('Opened cache');
                    return cache.addAll(urlsToCache);
                })
        );
    });

    self.addEventListener('fetch', function (ev) {
        ev.respondWith(
            caches.match(ev.request)
                .then(function (response) {
                    if (response) {
                        return response;
                    }
                    return fetch(ev.request);
                }
            )
        );
    });

    self.addEventListener('push', function (ev) {
        if (ev.data) {
            var data = ev.data.json();
            console.log(data);

            const promiseChain = self.registration.showNotification(data.title, data);

            ev.waitUntil(promiseChain);
        } else {
            console.log('This push event has no data.');
        }
    });

    // called when the notication is dismissed (by swiping or clicking the x)
    // this would typically be used to measure engagement using analytics
    // send some event data to google analytics or whatever
    self.addEventListener("notificationclose", function (ev) {
        const dismissedNotification = ev.notification;
        const promiseChain = notificationCloseAnalytics();
        ev.waitUntil(promiseChain);
    });

    self.addEventListener('notificationclick', function (ev) {
        const clickedNotification = ev.notification;
        const notificationData = clickedNotification.data;
        var url = '';

        // close the notification
        clickedNotification.close();

        // multiple actions
        if (!ev.action) {
            console.log('Normal Notification Click.');
            if (notificationData && notificationData.url) {
                url = notificationData.url;
            }
        } else {
            switch (ev.action) {
                case 'ignore':
                    console.log('User clicked on ignore.');
                    url = '/leads/ignore';
                    break;
                case 'buy':
                    console.log('User clicked on buy');
                    url = '/leads/purchase';
                    break;
                default:
                    console.log(`Unknown action clicked: '${ev.action}'`);
                    break;
            }
        }

        // need to use waitUntil so the ServiceWorker is kept alive long enough to do your stuff
        if (url && url.length > 0) {
            if (notificationData.listingId) {
                url = url + '/' + notificationData.listingId;
            }

            const urlToOpen = new URL(url, self.location.origin).href;
            console.log(urlToOpen);

            // this will find any existing windows or tabs with this url and focus it instead of opening a new one if possible
            const promiseChain = clients.matchAll({
                type: 'window',
                includeUncontrolled: true
            }).then((windowClients) => {
                let matchingClient = null;

                for (let i = 0; i < windowClients.length; i++) {
                    const windowClient = windowClients[i];
                    if (windowClient.url === urlToOpen) {
                        matchingClient = windowClient;
                        break;
                    }
                }

                if (matchingClient) {
                    return matchingClient.focus();
                } else {
                    clients.openWindow(urlToOpen);
                }
            })

            ev.waitUntil(promiseChain);
        }

        return;
    });

    function notificationCloseAnalytics(notification) {
        // do some shit and resolve the promise
        return;
    };

})();