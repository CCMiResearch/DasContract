import { SlowScroll } from "@drozdik.m/slow-scroll";

(window as any).MaterialBootstrapRazorComponents.SlowScroll = {
    AnchorScroll: function (className: string)
    {
        SlowScroll.AnchorScroll(className);
    },

    ToPx: function (top: number)
    {
        SlowScroll.ToPx(top);
    },

    To: function (element: HTMLElement)
    {
        SlowScroll.To(element);
    },

    ToFirst: function (selector: string)
    {
        SlowScroll.ToFirst(selector);
    },

    ToTop: function ()
    {
        SlowScroll.ToTop();
    }
}

