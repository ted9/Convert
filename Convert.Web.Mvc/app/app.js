angular.module("app", [])
.controller("appCtrl", ["$scope", "querySvc", "urlConfig", function($scope, querySvc, urlConfig) {
    $scope.username = "John Smith";
   
    $scope.$watch('amount', function(newValue, oldValue) {
        if (oldValue !== newValue) {
            convertAmount(newValue);
        }
    });
    function convertAmount(value) {
        if (value === null) {
            $scope.amountString = null;
        } else {
            var url = urlConfig.translate;
            url = String.format(url, $scope.username, value);
            querySvc.get(url).then(function (data) {
                $scope.amountString = data.AmountString;
            });

        }
    };
}])