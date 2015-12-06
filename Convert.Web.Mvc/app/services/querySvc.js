angular.module('app')
.service('querySvc', ['$http','$q', function($http, $q) {
    'use strict';
   
    var svc = {
        get: get,
        post: post,
        put: put
    };
    return svc;

    function get(url) {
        var promise = $http.get(url).then(function(response) {
            return response.data;
        }, function (response) {
            handleException(response);
            return $q.reject(response.data);
        });
        return promise;
    }
    function post(url, data) {       
    }

    function put(url, data) {
        
    }

    function handleException(response) {

        if (response.data === null || response.data === undefined ) {
            response.data = {};
        }
        response.data.httpStatusCode = response.status;

        if (response.status === 401 || response.status === 400 || response.status === 500) {
            var errorPageUrl = baseUrl + 'error/#/error';   // no router for this example, it's not working
            if (response) {
                sessionStorage.setItem('errorMessage', JSON.stringify(response.data));
            }
        }
    }

}])