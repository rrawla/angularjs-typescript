var WebApp;
(function (WebApp) {
    (function (Services) {
        var DataService = (function () {
            function DataService($http, $q) {
                this.$http = $http;
                this.$q = $q;
            }
            DataService.prototype.getTeams = function () {
                var self = this;
                self.teams = [];
                var deferred = self.$q.defer();
                self.$http.get("/api/teams").then(function (result) {
                    // Successful
                    angular.copy(result.data, self.teams);
                    deferred.resolve(self.teams);
                }, function () {
                    // Error
                    deferred.reject();
                });

                return deferred.promise;
            };

            DataService.prototype.getTeam = function (id) {
                var self = this;
                var found = null;
                $.each(self.teams, function (i, item) {
                    if (item.TeamId == id) {
                        found = item;
                        return false;
                    }
                });

                return found;
            };

            DataService.prototype.addTeam = function (newTeam) {
                var self = this;

                var deferred = self.$q.defer();
                self.$http.post("/api/teams", newTeam).then(function (result) {
                    // success
                    var newlyCreatedTeam = result.data;
                    self.teams.splice(0, 0, newlyCreatedTeam);
                    deferred.resolve(newlyCreatedTeam);
                }, function () {
                    // error
                    deferred.reject();
                });
                return deferred.promise;
            };

            DataService.prototype.saveTeam = function (saveTeam) {
                var self = this;

                var deferred = self.$q.defer();
                self.$http.put("/api/teams/" + saveTeam.TeamId, saveTeam).then(function (result) {
                    // success
                    deferred.resolve();
                }, function () {
                    // error
                    deferred.reject();
                });
                return deferred.promise;
            };
            return DataService;
        })();
        Services.DataService = DataService;

        DataService.$inject = ["$http", "$q"];
    })(WebApp.Services || (WebApp.Services = {}));
    var Services = WebApp.Services;
})(WebApp || (WebApp = {}));
//# sourceMappingURL=DataService.js.map
