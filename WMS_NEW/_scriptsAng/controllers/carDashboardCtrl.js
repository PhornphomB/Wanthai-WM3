function carDashboardCtrl($scope, carDashboardSvc, $rootScope, $timeout) {

    //$scope.Count = 0; // test refresh
    $scope.Datas = new Array();
    $scope.WorkType = "";
    $scope.ProcStatus = "";
    $scope.LastCarId = "";

    carDashboardSvc.initialize();

    $scope.angPostBackAsync = function (eventName, eventArgs) {

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        if (!Array.contains(prm._asyncPostBackControlIDs, eventName)) {
            prm._asyncPostBackControlIDs.push(eventName);
        }

        if (!Array.contains(prm._asyncPostBackControlClientIDs, eventName)) {
            prm._asyncPostBackControlClientIDs.push(eventName);
        }

        __doPostBack(eventName, eventArgs);
    };

    $scope.getCarDashboard = function (work_type, proc_status) {

        $scope.WorkType = work_type === "INB" ? "INBOUND" : work_type === "OUTB" ? "OUTBOUND" : '';
        $scope.ProcStatus = proc_status.replace(/_/g, " ");

        carDashboardSvc.getDashboardData(work_type, proc_status);
    };

    $scope.$parent.$on("getDashboardData", function (e, datas) {
        $scope.$apply(function () {

            if (datas !== null) {
                $scope.Datas = datas;
            }
        });
    });

    $scope.$parent.$on("setDashboardData", function (e, datas) {
        $scope.$apply(function () {

            if (datas !== null) {

                if ($scope.WorkType !== '' && $scope.ProcStatus !== '') {
                    $scope.Datas = datas.filter(function (row) {
                        return row.work_type === $scope.WorkType && row.order_process_status === $scope.ProcStatus;
                    });
                }
                else if ($scope.WorkType !== '') {
                    $scope.Datas = datas.filter(function (row) {
                        return row.work_type === $scope.WorkType;
                    });
                }
                else if ($scope.ProcStatus !== '') {
                    $scope.Datas = datas.filter(function (row) {
                        return row.order_process_status === $scope.ProcStatus;
                    });
                }
                else {
                    $scope.Datas = datas;
                }

                if ($scope.Datas.length > 0) {

                    var first = $scope.Datas[0];

                    if ($scope.LastCarId !== first.id && first.order_process_num > 0) {
                        $scope.LastCarId = first.id;
                        $scope.angPostBackAsync('_CAR_CLICK', first.id);
                    }
                }
            }

        });
    });
}