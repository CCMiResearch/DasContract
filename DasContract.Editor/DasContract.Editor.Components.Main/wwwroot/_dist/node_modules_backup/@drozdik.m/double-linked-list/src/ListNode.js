exports.__esModule = true;
//--------------------------------------------------
//----------LIST NODE-------------------------------
//--------------------------------------------------
var ListNode = /** @class */ (function () {
    /**
     * Creates new instance of list node
     * @param value Value of the node
     * @param next Reference to the next node
     * @param previous Reference to the previous node
     */
    function ListNode(value, next, previous) {
        if (next === void 0) { next = null; }
        if (previous === void 0) { previous = null; }
        this.next = null;
        this.previous = null;
        this.value = null;
        this.value = value;
        this.next = next;
        this.previous = previous;
    }
    return ListNode;
}());
exports.ListNode = ListNode;
