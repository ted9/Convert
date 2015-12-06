
(function () {
    'use strict';
    angular.module('app')
        .provider('urlConfig', function () {

        var urls = {
            translate: baseApiUrl + "convert/translate?username={0}&amount={1}",
        };
            
        this.addUrl = function (key, url) {
            if (urls[key]) {
                throw Error("Key " + key + " has been existed.");
            }
            urls[key] = url;
        }; 
        this.$get = function () {
            return urls;
        };
        });
}());
