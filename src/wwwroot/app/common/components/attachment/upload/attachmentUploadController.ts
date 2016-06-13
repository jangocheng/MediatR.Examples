/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import AzureBlobUploadFactory = Antares.Factories.AzureBlobUploadFactory;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class AttachmentUploadController {
        public componentId: string;
        public entityType: Enums.EntityTypeEnum;
        public enumDocumentType: Dto.EnumTypeCode;

        public attachmentTypes: any[];
        public file: File = null;
        public isFileCleared: boolean = false;
        public documentTypeId: string;

        private urlResource: ng.resource.IResourceClass<Common.Models.Resources.IAzureUploadUrlResource>;

        constructor(
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private componentRegistry: Core.Service.ComponentRegistry,
            private azureBlobUploadFactory: AzureBlobUploadFactory,
            private dataAccessService: Services.DataAccessService) {

            this.urlResource = dataAccessService.getAzureUploadUrlResource(this.entityType);

            componentRegistry.register(this, this.componentId);
        }

        isDataValid = (): boolean => {
            var form = this.$scope["attachmentUploadForm"];
            form.$setSubmitted();
            return form.$valid;
        }

        clearSelectedFile = () => {
            this.file = null;
            this.isFileCleared = true;
        };

        clearAttachmentForm = () => {
            this.file = null;
            this.isFileCleared = false;
            this.documentTypeId = null;

            var form = this.$scope["attachmentUploadForm"];
            form.$setPristine();
        };

        getAzureUploadUrl = (entityReferenceId: string) => {
            return this.urlResource
                .get({
                    documentTypeId: this.documentTypeId,
                    localeIsoCode: 'en',
                    entityReferenceId: entityReferenceId,
                    filename: this.file.name
                })
                .$promise;
        }

        uploadFile = (urlContainer: Dto.IAzureUploadUrlContainer) => {
            return this.azureBlobUploadFactory
                .uploadFile(this.file, urlContainer.url)
                .then(() => {
                    return urlContainer.externalDocumentId;
                });
            /*** ToDo: use alternative code below (for better way of passing resolved values form getAzureUploadUrl promise to createAttachment method, instead of "fake" then in uploadFile method
             * do it after sprint 5 demo :)
                .uploadFile(this.file, urlContainer.url);
            */
        }

        reject = () => {
            var uploadResult = this.$q.defer();
            uploadResult.reject();

            return uploadResult.promise;
        }

        createAttachment = (externalDocumentId: string) => {
            var attachment = new Business.Attachment();
            attachment.externalDocumentId = externalDocumentId;
            attachment.fileName = this.file.name;
            attachment.size = this.file.size;
            attachment.documentTypeId = this.documentTypeId;

            return attachment;
        }

        uploadAttachment = (entityReferenceId: string): ng.IPromise<Business.Attachment> => {
            if (this.file === null) {
                this.isFileCleared = true;
            }

            if (!this.isDataValid() || this.file === null) {
                return this.reject();
            }

            return this.getAzureUploadUrl(entityReferenceId)
                .then(this.uploadFile, this.reject)
                .then(this.createAttachment);

            /*** ToDo: use alternative code below (for better way of passing resolved values form getAzureUploadUrl promise to createAttachment method, instead of "fake" then in uploadFile method
             * do it after sprint 5 demo :)
            var getAzureUploadUrl = this.getAzureUploadUrl(entityReferenceId);

            return this.$q
                .all([
                    getAzureUploadUrl,
                    getAzureUploadUrl.then(this.uploadFile, this.showError)
                ])
                .then((values: [Dto.IAzureUploadUrlContainer]) =>
                    this.createAttachment(<string>values[0].externalDocumentId));
            */
        };
    }

    angular.module('app').controller('AttachmentUploadController', AttachmentUploadController);
}