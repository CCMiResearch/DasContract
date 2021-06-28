function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

import EditingManager from 'dmn-js-shared/lib/base/EditingManager';
import DrdModeler from 'dmn-js-drd/lib/Modeler';
import DecisionTableEditor from 'dmn-js-decision-table/lib/Editor';
import LiteralExpressionEditor from 'dmn-js-literal-expression/lib/Editor';
import { is } from 'dmn-js-shared/lib/util/ModelUtil';
import { containsDi } from 'dmn-js-shared/lib/util/DiUtil';
/**
 * The dmn editor.
 */

var Modeler =
/*#__PURE__*/
function (_EditingManager) {
  _inherits(Modeler, _EditingManager);

  function Modeler() {
    _classCallCheck(this, Modeler);

    return _possibleConstructorReturn(this, _getPrototypeOf(Modeler).apply(this, arguments));
  }

  _createClass(Modeler, [{
    key: "_getViewProviders",
    value: function _getViewProviders() {
      return [{
        id: 'drd',
        constructor: DrdModeler,
        opens: 'dmn:Definitions'
      }, {
        id: 'decisionTable',
        constructor: DecisionTableEditor,
        opens: function opens(element) {
          return is(element, 'dmn:Decision') && is(element.decisionLogic, 'dmn:DecisionTable');
        }
      }, {
        id: 'literalExpression',
        constructor: LiteralExpressionEditor,
        opens: function opens(element) {
          return is(element, 'dmn:Decision') && is(element.decisionLogic, 'dmn:LiteralExpression');
        }
      }];
    }
  }, {
    key: "_getInitialView",
    value: function _getInitialView(views) {
      var definitionsView;

      for (var i = 0; i < views.length; i++) {
        var view = views[i];
        var el = view.element;

        if (is(el, 'dmn:Decision')) {
          return view;
        }

        if (is(el, 'dmn:Definitions')) {
          definitionsView = view;

          if (containsDi(el)) {
            return view;
          }
        }
      }

      return definitionsView || views[0];
    }
  }]);

  return Modeler;
}(EditingManager);

export { Modeler as default };
//# sourceMappingURL=Modeler.js.map