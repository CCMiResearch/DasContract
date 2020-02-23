exports.__esModule = true;
var slow_scroll_1 = require("@drozdik.m/slow-scroll");
window.MaterialBootstrapRazorComponents.SlowScroll = {
    AnchorScroll: function (className) {
        slow_scroll_1.SlowScroll.AnchorScroll(className);
    },
    ToPx: function (top) {
        slow_scroll_1.SlowScroll.ToPx(top);
    },
    To: function (element) {
        slow_scroll_1.SlowScroll.To(element);
    },
    ToFirst: function (selector) {
        slow_scroll_1.SlowScroll.ToFirst(selector);
    },
    ToTop: function () {
        slow_scroll_1.SlowScroll.ToTop();
    }
};
