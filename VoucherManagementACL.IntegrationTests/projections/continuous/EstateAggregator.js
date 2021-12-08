//var fromAll = fromAll || require("../../node_modules/esprojection-testing-framework").scope.fromAll;
//var linkTo = linkTo || require("../../node_modules/esprojection-testing-framework").scope.linkTo;

isEstateEvent = (e) => { return (e.data && e.data.estateId); }
isAnEstateCreatedEvent = (e) => { return compareEventTypeSafely(e.eventType, 'EstateCreatedEvent') };

isAMerchantFeeAddedToTransactionEvent = (e) => { return compareEventTypeSafely(e.eventType, 'MerchantFeeAddedToTransactionEvent') };
isAServiceProviderFeeAddedToTransactionEvent = (e) => { return compareEventTypeSafely(e.eventType, 'ServiceProviderFeeAddedToTransactionEvent') };

compareEventTypeSafely = (sourceEventType, targetEventType) => { return (sourceEventType.toUpperCase() === targetEventType.toUpperCase()); }

ignoreEvent = (e) => isAServiceProviderFeeAddedToTransactionEvent(e) | isAMerchantFeeAddedToTransactionEvent(e);

isInvalidEvent = (e) => (e === null || e === undefined || e.data === undefined);

isTruncated = function (metadata) {
    if (metadata && metadata['$v']) {
        var parts = metadata['$v'].split(":");
        var projectionEpoch = parts[1];

        return (projectionEpoch < 0);
    }
    return false;
};

fromAll()
    .when({
            $init: function (s, e) {
                return { estates: {} }
            },
            $any: function (s, e) {
                if (isTruncated(e)) return;

                if (isEstateEvent(e)) {
                    if (ignoreEvent(e)) return;

                    if (isAnEstateCreatedEvent(e)) {
                        s.estates[e.data.estateId] = {
                            name: e.data.estateName.replace(/-/gi, "").replace(/ /g, "")
                        };
                    }

                    linkTo(s.estates[e.data.estateId].name, e);
                }
            }
        }
    );