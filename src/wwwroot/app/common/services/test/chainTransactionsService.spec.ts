﻿/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Commands = Antares.Common.Models.Commands;
    import Enums = Antares.Common.Models.Enums;

    describe('ChainTransationsService', () => {
        var service: Services.ChainTransationsService;
        var requirementService: Antares.Requirement.RequirementService;
        var activityService: Services.ActivityService;

        var generator: TestHelpers.ChainTransactionGenerator;

        beforeEach(inject((chainTransationsService: Services.ChainTransationsService, 
        _requirementService_: Antares.Requirement.RequirementService, 
        _activityService_: Services.ActivityService) => {
            service = chainTransationsService;
            requirementService = _requirementService_;
            activityService = _activityService_;

            spyOn(requirementService, 'updateRequirement');
            spyOn(activityService, 'updateActivity');

            generator =  new TestHelpers.ChainTransactionGenerator();
        }));

        describe('when addChain is invoked', () => {
            it('then it should invoke service with new chain', () => {
                // arrange
                var command = createCommand();
                var chain = generator.generateDto();
                
                // act
                service.addChain(chain, command, Enums.OfferChainsType.Activity);

                // assert
                var spyMethod = <jasmine.Spy>activityService.updateActivity;
                var updateCommand = <Commands.IChainTransactionCommand> spyMethod.calls.argsFor(0)[0];
                var newChain = updateCommand.chainTransactions[updateCommand.chainTransactions.length - 1];
                expect(newChain).toBe(chain);
            });

            it('when Activity offerChainType passed then it should invoke activity service', () => {
                // arrange
                var command = createCommand();
                var chain = generator.generateDto();
                
                // act
                service.addChain(chain, command, Enums.OfferChainsType.Activity);

                // assert
                expect(activityService.updateActivity).toHaveBeenCalled();
            });

            it('when Requirement offerChainType passed then it should invoke requirement service', () => {
                // arrange
                var command = createCommand();
                var chain = generator.generateDto();
                
                // act
                service.addChain(chain, command, Enums.OfferChainsType.Requirement);

                // assert
                expect(requirementService.updateRequirement).toHaveBeenCalled();
            });
        });

        describe('when editChain is invoked', () => {
            it('then it should invoke service with edited chain', () => {
                // arrange
                var command = createCommand();
                var chainToEdit = generator.generateDto();
                var targetChain = command.chainTransactions[1];
                chainToEdit.id = targetChain.id;
                
                // act
                service.editChain(chainToEdit, command, Enums.OfferChainsType.Activity);

                // assert
                var spyMethod = <jasmine.Spy>activityService.updateActivity;
                var updateCommand = <Commands.IChainTransactionCommand> spyMethod.calls.argsFor(0)[0];
                var editedChain = updateCommand.chainTransactions.filter(c => c.id === targetChain.id)[0];
                expect(editedChain).toEqual(chainToEdit);
            });

            it('when Activity offerChainType passed then it should invoke activity service', () => {
                // arrange
                var command = createCommand();
                var chain = generator.generateDto();
                
                // act
                service.editChain(chain, command, Enums.OfferChainsType.Activity);

                // assert
                expect(activityService.updateActivity).toHaveBeenCalled();
            });

            it('when Requirement offerChainType passed then it should invoke requirement service', () => {
                // arrange
                var command = createCommand();
                var chain = generator.generateDto();
                
                // act
                service.editChain(chain, command, Enums.OfferChainsType.Requirement);

                // assert
                expect(requirementService.updateRequirement).toHaveBeenCalled();
            });
        });

        describe('when removeChain is invoked', () => {
            it('then it should invoke service without removed chain', () => {
                // arrange
                var command = createCommand();
                var chainToRemove = generator.generateDto();
                var targetChain = command.chainTransactions[1];
                chainToRemove.id = targetChain.id;
                
                // act
                service.removeChain(chainToRemove, command, Enums.OfferChainsType.Activity);

                // assert
                var spyMethod = <jasmine.Spy>activityService.updateActivity;
                var updateCommand = <Commands.IChainTransactionCommand> spyMethod.calls.argsFor(0)[0];
                var removedChain = command.chainTransactions.filter(c => c.id === targetChain.id)[0];
                expect(removedChain).toBeUndefined();
            });

            it('when Activity offerChainType passed then it should invoke activity service', () => {
                // arrange
                var command = createCommand();
                var chain = generator.generateDto();
                
                // act
                service.removeChain(chain, command, Enums.OfferChainsType.Activity);

                // assert
                expect(activityService.updateActivity).toHaveBeenCalled();
            });

            it('when Requirement offerChainType passed then it should invoke requirement service', () => {
                // arrange
                var command = createCommand();
                var chain = generator.generateDto();
                
                // act
                service.removeChain(chain, command, Enums.OfferChainsType.Requirement);

                // assert
                expect(requirementService.updateRequirement).toHaveBeenCalled();
            });
        });

        var createCommand = (): Commands.IChainTransactionCommand => {
            return {
                chainTransactions: [
                    generator.generateDto(),
                    generator.generateDto(),
                    generator.generateDto()
                ]
            }
        }
    });
}