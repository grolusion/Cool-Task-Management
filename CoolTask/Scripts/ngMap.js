var app = angular.module('tmsApp', ['uiGmapgoogle-maps']);
app.controller('mapController', function ($scope, $http) {
    debugger;
    //this is for default map focus when load first time
    $scope.map = { center: { latitude: 22.590406, longitude: 88.366034 }, zoom: 18 }

    $scope.markers = [];
    $scope.locations = [];

    //Populate all location
    $http.get('/Account/GetAllLocation').then(function (data) {
        debugger;
        $scope.locations = data.data;
        alert(data.data)
    }, function () {
        alert('Error');
    });

    //get marker info
    $scope.ShowLocation = function (CompanyId) {
        debugger;
        $http.get('/Account/GetMarkerInfo', {
            params: {
                CompanyId: CompanyId
            }
        }).then(function (data) {
            //clear markers 
            $scope.markers = [];
            $scope.markers.push({
                id: data.data.CompanyId,
                coords: { latitude: data.data.Lat, longitude: data.data.Long },
                title: data.data.CompanyName,
                address: data.data.Alamat,
                image : data.data.ImagePath
            });

            //set map focus to center
            $scope.map.center.latitude = data.data.Lat;
            $scope.map.center.longitude = data.data.Long;

        }, function () {
            alert('Error');
        });
    }

    //Show / Hide marker on map
    $scope.windowOptions = {
        show: true
    };

});