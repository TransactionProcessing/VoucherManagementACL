//var fromAll = fromAll || require("../../node_modules/esprojection-testing-framework").scope.fromAll;
//var linkTo = linkTo || require("../../node_modules/esprojection-testing-framework").scope.linkTo;

isValidEvent = function (e) {

    if (e) {
        if (e.data) {
            if (e.isJson) {
                if (e.eventType !== "$metadata") {
                    return true;
                }
            }
        }
    }

    return false;
};

getMerchantId = function (e) {
    if (e.data.merchantId === undefined) {
        return null;
    }
    return e.data.merchantId;
};

fromAll()
    .when({
        $any: function (s, e) {
            if (isValidEvent(e)) {
                var merchantId = getMerchantId(e);
                if (merchantId !== null) {
                    s.merchantId = merchantId;
                    var streamName = "MerchantArchive-" + merchantId.replace(/-/gi, "");
                    s.streamName = streamName;
                    linkTo(streamName, e);
                }
            }
        }
    });