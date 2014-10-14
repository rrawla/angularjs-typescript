module WebApp.Controllers {

    export interface IControllerScopeBase extends ng.IScope {
        PageTitle: string
    }

    export interface ITeamsControllerScope extends IControllerScopeBase {
        easternTeams: Models.Team[];
        westernTeams: Models.Team[];
        editTeam(id: number);
        isBusy: boolean;
    }
    export class TeamsController {
        
        constructor(private $scope: ITeamsControllerScope,
            private dataService: WebApp.Services.DataService,
            private $location: ng.ILocationService) {
            var self: TeamsController = this;
            $scope.PageTitle = "Teams";
            $scope.isBusy = true;
            dataService.getTeams()
                .then(data => {
                    $scope.easternTeams = [];
                    $scope.westernTeams = [];

                    $.each(data, function (i, item) {
                        if (item.Division == "Eastern") {
                            $scope.easternTeams.push(item);
                        } else {
                            $scope.westernTeams.push(item);
                        }
                    });
                    $scope.isBusy = false;
                });

            $scope.editTeam = (id: number) => {
                $location.url("/team/" + id);
            };
        }
    }

    TeamController.$inject = ["$scope", "$routeParams", "dataService", "$location"];
} 