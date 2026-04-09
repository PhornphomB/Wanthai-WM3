app.service('carDashboardSvc', function ($, $rootScope) {

    var hubConn = $.hubConnection($rootScope.hubConnectUrl);
    var proxyThis = hubConn.createHubProxy('carDashboard');

    function init() {
        getDashboardData('', '');
    }

    var initialize = function () {
        hubConn.start().done(init);
    };

    var getDashboardData = function (work_type, proc_status) {

        proxyThis.invoke('GetDashboardData', work_type, proc_status).done(function (datas) {
            $rootScope.$emit("getDashboardData", datas);
        }).fail(function (error) {
        });
    };

    proxyThis.on('SetDashboardData', function (data) {
        $rootScope.$emit("setDashboardData", data);
    });

    return {
        initialize: initialize,
        getDashboardData: getDashboardData
    };
});