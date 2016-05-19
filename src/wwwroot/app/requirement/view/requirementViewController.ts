/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.View {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class RequirementViewController extends Core.WithPanelsBaseController {
        requirement: Business.Requirement;
        viewingAddPanelVisible: boolean = false;
        viewingPreviewPanelVisible: boolean = true;
        loadingActivities: boolean = false;
        saveViewingBusy: boolean = false;
        addOfferBusy: boolean = false;
        userData: Dto.IUserData;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope) {

            super(componentRegistry, $scope);
        }

        showNotesPanel = () => {
            this.components.noteAdd().clearNote();
            this.showPanel(this.components.panels.notes);
        }

        showActivitiesPanel = () => {
            this.loadingActivities = true;
            this.components.activitiesList()
                .loadActivities()
                .finally(() => { this.loadingActivities = false; });

            this.components.viewingAdd().clearViewingAdd();
            this.showPanel(this.components.panels.configureViewingsSidePanel);
            this.viewingAddPanelVisible = false;
        }

        showViewingAddPanel = () => {
            var selectedActivity: Dto.IActivityQueryResult = this.components
                .activitiesList()
                .getSelectedActivity();

            if (selectedActivity === null || selectedActivity === undefined) {
                return;
            }

            this.components.viewingAdd().clearViewingAdd();
            this.components.viewingAdd().setActivity(selectedActivity);

            this.viewingAddPanelVisible = true;

            this.showPanel(this.components.panels.configureViewingsSidePanel);
        }

        showViewingPreviewPanel = () => {
            this.showPanel(this.components.panels.previewViewingsSidePanel);
        }

        cancelConfigureViewings() {
            this.components.panels.configureViewingsSidePanel().hide();
        }

        cancelViewingAdd() {
            this.viewingAddPanelVisible = false;
        }

        goBackToPreviewViewing() {
            this.viewingPreviewPanelVisible = true;
        }

        cancelViewingPreview() {
            this.hidePanels();
        }

        defineComponentIds(): void {
            this.componentIds = {
                noteAddId: 'requirementView:requirementNoteAddComponent',
                noteListId: 'requirementView:requirementNoteListComponent',
                notesSidePanelId: 'requirementView:notesSidePanelComponent',
                activitiesListId: 'addRequirement:activitiesListComponent',
                viewingAddId: 'addRequirement:viewingAddComponent',
                viewingEditId: 'requirementView:viewingEditComponent',
                viewingPreviewId: 'addRequirement:viewingPreviewComponent',
                configureViewingsSidePanelId: 'addRequirement:configureViewingsSidePanelComponent',
                previewViewingSidePanelId: 'addRequirement:previewViewingSidePanelComponent',
                offerAddId: 'requirementView:addOfferComponent',
                offerSidePanelId: 'requirementView:offerSidePanelComponent',
                offerPreviewId: 'requirementView:offerPreviewComponent',
                offerPreviewSidePanelId: 'requirementView:offerPreviewSidePanelComponent'
            }
        }

        defineComponents(): void {
            this.components = {
                noteAdd: () => { return this.componentRegistry.get(this.componentIds.noteAddId); },
                noteList: () => { return this.componentRegistry.get(this.componentIds.noteListId); },
                activitiesList: () => { return this.componentRegistry.get(this.componentIds.activitiesListId); },
                viewingAdd: () => { return this.componentRegistry.get(this.componentIds.viewingAddId); },
                viewingEdit: () => { return this.componentRegistry.get(this.componentIds.viewingEditId); },
                viewingPreview: () => { return this.componentRegistry.get(this.componentIds.viewingPreviewId); },
                offerPreview: () => { return this.componentRegistry.get(this.componentIds.offerPreviewId); },
                addOffer: () => { return this.componentRegistry.get(this.componentIds.offerAddId); },
                panels: {
                    notes: () => { return this.componentRegistry.get(this.componentIds.notesSidePanelId); },
                    configureViewingsSidePanel: () => { return this.componentRegistry.get(this.componentIds.configureViewingsSidePanelId); },
                    offerPreviewSidePanel: () => { return this.componentRegistry.get(this.componentIds.offerPreviewSidePanelId)},
                    previewViewingsSidePanel: () => { return this.componentRegistry.get(this.componentIds.previewViewingSidePanelId); },
                    addOffer: () => { return this.componentRegistry.get(this.componentIds.offerSidePanelId); }
                }
            }
        }

        showViewingPreview = (viewing: Common.Models.Business.Viewing) => {
            this.components.viewingPreview().clearViewingPreview();
            this.components.viewingPreview().setViewing(viewing);
            this.showPanel(this.components.panels.previewViewingsSidePanel);
            this.viewingPreviewPanelVisible = true;
        }

        showViewingEdit = () => {
            var viewing = this.components.viewingPreview().getViewing();
            this.components.viewingEdit().setViewing(viewing);
            this.viewingPreviewPanelVisible = false;
        }

        showOfferPreview = (offer: Common.Models.Business.Offer) => {
            this.components.offerPreview().clearOfferPreview();
            this.components.offerPreview().setOffer(offer);
            this.showPanel(this.components.panels.offerPreviewSidePanel);
        }

        saveViewing() {
            this.saveViewingBusy = true;
            this.components.viewingAdd()
                .saveViewing()
                .then((viewing: Common.Models.Dto.IViewing) => {
                    var viewingModel = new Business.Viewing(viewing);
                    this.requirement.viewings.push(viewingModel);
                    this.requirement.groupViewings(this.requirement.viewings);
                    this.hidePanels();
                }).finally(() => {
                    this.saveViewingBusy = false;
                });
        }

        saveEditedViewing() {
            this.saveViewingBusy = true;
            this.components.viewingEdit()
                .saveViewing()
                .then((viewing: Common.Models.Dto.IViewing) => {
                    var editedViewing = this.components.viewingPreview().getViewing();
                    editedViewing = angular.copy(new Business.Viewing(viewing), editedViewing);
                    this.requirement.groupViewings(this.requirement.viewings);
                    this.hidePanels();
                }).finally(() => {
                    this.saveViewingBusy = false;
                });
        }

        showAddOfferPanel = (viewing: Dto.IViewing) => {
            if (this.components.panels.addOffer().visible) return;
            var addOfferComponent = this.components.addOffer();

            addOfferComponent.activity = viewing.activity;
            addOfferComponent.reset();
            
            this.showPanel(this.components.panels.addOffer);
        }

        cancelSaveOffer = () => {
            this.hidePanels();
        }

        saveOffer = () => {
            this.addOfferBusy = true;
            this.components.addOffer()
                .saveOffer()
                .then(() => {
                    this.hidePanels();
                })
                .finally(() => {
                    this.addOfferBusy = false;
                });
        }
    }

    angular.module('app').controller('requirementViewController', RequirementViewController);
}