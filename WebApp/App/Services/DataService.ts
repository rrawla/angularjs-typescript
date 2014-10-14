module WebApp.Services {
    interface INewTeamResult {
        data: Models.Team;
    }

    export class DataService {
        teams: Models.Team[];

        constructor(private $http: ng.IHttpService, private $q: ng.IQService) {

        }

        getTeams(): ng.IPromise<Models.Team[]> {
            var self: DataService = this;
            self.teams = [];
            var deferred = self.$q.defer();
            self.$http.get("/api/teams")
                .then(function (result) {
                    // Successful
                    angular.copy(result.data, self.teams);
                    deferred.resolve(self.teams);
                },
                function () {
                    // Error
                    deferred.reject();
                });

            return deferred.promise;
        }

        getTeam(id: number): Models.Team {
            var self: DataService = this;
            var found: Models.Team = null;
            $.each(self.teams, function (i, item) {
                if (item.TeamId == id) {
                    found = item;
                    return false;
                }
            });

            return found;
        }

        addTeam(newTeam: Models.Team): ng.IPromise<Models.Team> {
            var self: DataService = this;

            var deferred = self.$q.defer();
            self.$http.post("/api/teams", newTeam)
                .then(function (result: INewTeamResult) {
                    // success
                    var newlyCreatedTeam: Models.Team = result.data;
                    self.teams.splice(0, 0, newlyCreatedTeam);
                    deferred.resolve(newlyCreatedTeam);
                },
                function () {
                    // error
                    deferred.reject();
                });
            return deferred.promise;
        }

        saveTeam(saveTeam: Models.Team): ng.IPromise<Models.Team> {
            var self: DataService = this;

            var deferred = self.$q.defer();
            self.$http.put("/api/teams/" + saveTeam.TeamId, saveTeam)
                .then(function (result: INewTeamResult) {
                    // success
                    deferred.resolve();
                },
                function () {
                    // error
                    deferred.reject();
                });
            return deferred.promise;
        }
    }

    DataService.$inject = ["$http", "$q"];
} 