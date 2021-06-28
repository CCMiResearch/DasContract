/*!
 * dmn-js - dmn-viewer v10.2.1
 *
 * Copyright (c) 2014-present, camunda Services GmbH
 *
 * Released under the bpmn.io license
 * http://bpmn.io/license
 *
 * Source Code: https://github.com/bpmn-io/dmn-js
 *
 * Date: 2021-06-04
 */
(function (global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
  typeof define === 'function' && define.amd ? define(factory) :
  (global = global || self, global.DmnJS = factory());
}(this, (function () { 'use strict';

  function _typeof(obj) {
    if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") {
      _typeof = function (obj) {
        return typeof obj;
      };
    } else {
      _typeof = function (obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj;
      };
    }

    return _typeof(obj);
  }

  function _classCallCheck(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties(Constructor, staticProps);
    return Constructor;
  }

  function _inherits(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf(subClass, superClass);
  }

  function _getPrototypeOf(o) {
    _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf(o);
  }

  function _setPrototypeOf(o, p) {
    _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf(o, p);
  }

  function _assertThisInitialized(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _possibleConstructorReturn(self, call) {
    if (call && (typeof call === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized(self);
  }

  /**
   * Flatten array, one level deep.
   *
   * @param {Array<?>} arr
   *
   * @return {Array<?>}
   */

  var nativeToString = Object.prototype.toString;
  var nativeHasOwnProperty = Object.prototype.hasOwnProperty;

  function isUndefined(obj) {
    return obj === undefined;
  }

  function isDefined(obj) {
    return obj !== undefined;
  }

  function isArray(obj) {
    return nativeToString.call(obj) === '[object Array]';
  }

  function isObject(obj) {
    return nativeToString.call(obj) === '[object Object]';
  }

  function isNumber(obj) {
    return nativeToString.call(obj) === '[object Number]';
  }

  function isFunction(obj) {
    var tag = nativeToString.call(obj);
    return tag === '[object Function]' || tag === '[object AsyncFunction]' || tag === '[object GeneratorFunction]' || tag === '[object AsyncGeneratorFunction]' || tag === '[object Proxy]';
  }

  function isString(obj) {
    return nativeToString.call(obj) === '[object String]';
  }
  /**
   * Return true, if target owns a property with the given key.
   *
   * @param {Object} target
   * @param {String} key
   *
   * @return {Boolean}
   */


  function has(target, key) {
    return nativeHasOwnProperty.call(target, key);
  }
  /**
   * Find element in collection.
   *
   * @param  {Array|Object} collection
   * @param  {Function|Object} matcher
   *
   * @return {Object}
   */


  function find(collection, matcher) {
    matcher = toMatcher(matcher);
    var match;
    forEach(collection, function (val, key) {
      if (matcher(val, key)) {
        match = val;
        return false;
      }
    });
    return match;
  }
  /**
   * Find element in collection.
   *
   * @param  {Array|Object} collection
   * @param  {Function} matcher
   *
   * @return {Array} result
   */


  function filter(collection, matcher) {
    var result = [];
    forEach(collection, function (val, key) {
      if (matcher(val, key)) {
        result.push(val);
      }
    });
    return result;
  }
  /**
   * Iterate over collection; returning something
   * (non-undefined) will stop iteration.
   *
   * @param  {Array|Object} collection
   * @param  {Function} iterator
   *
   * @return {Object} return result that stopped the iteration
   */


  function forEach(collection, iterator) {
    var val, result;

    if (isUndefined(collection)) {
      return;
    }

    var convertKey = isArray(collection) ? toNum : identity;

    for (var key in collection) {
      if (has(collection, key)) {
        val = collection[key];
        result = iterator(val, convertKey(key));

        if (result === false) {
          return val;
        }
      }
    }
  }
  /**
   * Reduce collection, returning a single result.
   *
   * @param  {Object|Array} collection
   * @param  {Function} iterator
   * @param  {Any} result
   *
   * @return {Any} result returned from last iterator
   */


  function reduce(collection, iterator, result) {
    forEach(collection, function (value, idx) {
      result = iterator(result, value, idx);
    });
    return result;
  }
  /**
   * Return true if every element in the collection
   * matches the criteria.
   *
   * @param  {Object|Array} collection
   * @param  {Function} matcher
   *
   * @return {Boolean}
   */


  function every(collection, matcher) {
    return !!reduce(collection, function (matches, val, key) {
      return matches && matcher(val, key);
    }, true);
  }
  /**
   * Transform a collection into another collection
   * by piping each member through the given fn.
   *
   * @param  {Object|Array}   collection
   * @param  {Function} fn
   *
   * @return {Array} transformed collection
   */


  function map(collection, fn) {
    var result = [];
    forEach(collection, function (val, key) {
      result.push(fn(val, key));
    });
    return result;
  }
  /**
   * Create an object pattern matcher.
   *
   * @example
   *
   * const matcher = matchPattern({ id: 1 });
   *
   * var element = find(elements, matcher);
   *
   * @param  {Object} pattern
   *
   * @return {Function} matcherFn
   */


  function matchPattern(pattern) {
    return function (el) {
      return every(pattern, function (val, key) {
        return el[key] === val;
      });
    };
  }

  function toMatcher(matcher) {
    return isFunction(matcher) ? matcher : function (e) {
      return e === matcher;
    };
  }

  function identity(arg) {
    return arg;
  }

  function toNum(arg) {
    return Number(arg);
  }
  /**
   * Debounce fn, calling it only once if
   * the given time elapsed between calls.
   *
   * @param  {Function} fn
   * @param  {Number} timeout
   *
   * @return {Function} debounced function
   */


  function debounce(fn, timeout) {
    var timer;
    var lastArgs;
    var lastThis;
    var lastNow;

    function fire() {
      var now = Date.now();
      var scheduledDiff = lastNow + timeout - now;

      if (scheduledDiff > 0) {
        return schedule(scheduledDiff);
      }

      fn.apply(lastThis, lastArgs);
      timer = lastNow = lastArgs = lastThis = undefined;
    }

    function schedule(timeout) {
      timer = setTimeout(fire, timeout);
    }

    return function () {
      lastNow = Date.now();

      for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
        args[_key] = arguments[_key];
      }

      lastArgs = args;
      lastThis = this; // ensure an execution is scheduled

      if (!timer) {
        schedule(timeout);
      }
    };
  }
  /**
   * Bind function against target <this>.
   *
   * @param  {Function} fn
   * @param  {Object}   target
   *
   * @return {Function} bound function
   */


  function bind(fn, target) {
    return fn.bind(target);
  }

  function _extends() {
    _extends = Object.assign || function (target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = arguments[i];

        for (var key in source) {
          if (Object.prototype.hasOwnProperty.call(source, key)) {
            target[key] = source[key];
          }
        }
      }

      return target;
    };

    return _extends.apply(this, arguments);
  }
  /**
   * Convenience wrapper for `Object.assign`.
   *
   * @param {Object} target
   * @param {...Object} others
   *
   * @return {Object} the target
   */


  function assign(target) {
    for (var _len = arguments.length, others = new Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
      others[_key - 1] = arguments[_key];
    }

    return _extends.apply(void 0, [target].concat(others));
  }
  /**
   * Pick given properties from the target object.
   *
   * @param {Object} target
   * @param {Array} properties
   *
   * @return {Object} target
   */


  function pick(target, properties) {
    var result = {};
    var obj = Object(target);
    forEach(properties, function (prop) {
      if (prop in obj) {
        result[prop] = target[prop];
      }
    });
    return result;
  }

  var FN_REF = '__fn';
  var DEFAULT_PRIORITY = 1000;
  var slice = Array.prototype.slice;
  /**
   * A general purpose event bus.
   *
   * This component is used to communicate across a diagram instance.
   * Other parts of a diagram can use it to listen to and broadcast events.
   *
   *
   * ## Registering for Events
   *
   * The event bus provides the {@link EventBus#on} and {@link EventBus#once}
   * methods to register for events. {@link EventBus#off} can be used to
   * remove event registrations. Listeners receive an instance of {@link Event}
   * as the first argument. It allows them to hook into the event execution.
   *
   * ```javascript
   *
   * // listen for event
   * eventBus.on('foo', function(event) {
   *
   *   // access event type
   *   event.type; // 'foo'
   *
   *   // stop propagation to other listeners
   *   event.stopPropagation();
   *
   *   // prevent event default
   *   event.preventDefault();
   * });
   *
   * // listen for event with custom payload
   * eventBus.on('bar', function(event, payload) {
   *   console.log(payload);
   * });
   *
   * // listen for event returning value
   * eventBus.on('foobar', function(event) {
   *
   *   // stop event propagation + prevent default
   *   return false;
   *
   *   // stop event propagation + return custom result
   *   return {
   *     complex: 'listening result'
   *   };
   * });
   *
   *
   * // listen with custom priority (default=1000, higher is better)
   * eventBus.on('priorityfoo', 1500, function(event) {
   *   console.log('invoked first!');
   * });
   *
   *
   * // listen for event and pass the context (`this`)
   * eventBus.on('foobar', function(event) {
   *   this.foo();
   * }, this);
   * ```
   *
   *
   * ## Emitting Events
   *
   * Events can be emitted via the event bus using {@link EventBus#fire}.
   *
   * ```javascript
   *
   * // false indicates that the default action
   * // was prevented by listeners
   * if (eventBus.fire('foo') === false) {
   *   console.log('default has been prevented!');
   * };
   *
   *
   * // custom args + return value listener
   * eventBus.on('sum', function(event, a, b) {
   *   return a + b;
   * });
   *
   * // you can pass custom arguments + retrieve result values.
   * var sum = eventBus.fire('sum', 1, 2);
   * console.log(sum); // 3
   * ```
   */

  function EventBus() {
    this._listeners = {}; // cleanup on destroy on lowest priority to allow
    // message passing until the bitter end

    this.on('diagram.destroy', 1, this._destroy, this);
  }
  /**
   * Register an event listener for events with the given name.
   *
   * The callback will be invoked with `event, ...additionalArguments`
   * that have been passed to {@link EventBus#fire}.
   *
   * Returning false from a listener will prevent the events default action
   * (if any is specified). To stop an event from being processed further in
   * other listeners execute {@link Event#stopPropagation}.
   *
   * Returning anything but `undefined` from a listener will stop the listener propagation.
   *
   * @param {string|Array<string>} events
   * @param {number} [priority=1000] the priority in which this listener is called, larger is higher
   * @param {Function} callback
   * @param {Object} [that] Pass context (`this`) to the callback
   */

  EventBus.prototype.on = function (events, priority, callback, that) {
    events = isArray(events) ? events : [events];

    if (isFunction(priority)) {
      that = callback;
      callback = priority;
      priority = DEFAULT_PRIORITY;
    }

    if (!isNumber(priority)) {
      throw new Error('priority must be a number');
    }

    var actualCallback = callback;

    if (that) {
      actualCallback = bind(callback, that); // make sure we remember and are able to remove
      // bound callbacks via {@link #off} using the original
      // callback

      actualCallback[FN_REF] = callback[FN_REF] || callback;
    }

    var self = this;
    events.forEach(function (e) {
      self._addListener(e, {
        priority: priority,
        callback: actualCallback,
        next: null
      });
    });
  };
  /**
   * Register an event listener that is executed only once.
   *
   * @param {string} event the event name to register for
   * @param {number} [priority=1000] the priority in which this listener is called, larger is higher
   * @param {Function} callback the callback to execute
   * @param {Object} [that] Pass context (`this`) to the callback
   */


  EventBus.prototype.once = function (event, priority, callback, that) {
    var self = this;

    if (isFunction(priority)) {
      that = callback;
      callback = priority;
      priority = DEFAULT_PRIORITY;
    }

    if (!isNumber(priority)) {
      throw new Error('priority must be a number');
    }

    function wrappedCallback() {
      wrappedCallback.__isTomb = true;
      var result = callback.apply(that, arguments);
      self.off(event, wrappedCallback);
      return result;
    } // make sure we remember and are able to remove
    // bound callbacks via {@link #off} using the original
    // callback


    wrappedCallback[FN_REF] = callback;
    this.on(event, priority, wrappedCallback);
  };
  /**
   * Removes event listeners by event and callback.
   *
   * If no callback is given, all listeners for a given event name are being removed.
   *
   * @param {string|Array<string>} events
   * @param {Function} [callback]
   */


  EventBus.prototype.off = function (events, callback) {
    events = isArray(events) ? events : [events];
    var self = this;
    events.forEach(function (event) {
      self._removeListener(event, callback);
    });
  };
  /**
   * Create an EventBus event.
   *
   * @param {Object} data
   *
   * @return {Object} event, recognized by the eventBus
   */


  EventBus.prototype.createEvent = function (data) {
    var event = new InternalEvent();
    event.init(data);
    return event;
  };
  /**
   * Fires a named event.
   *
   * @example
   *
   * // fire event by name
   * events.fire('foo');
   *
   * // fire event object with nested type
   * var event = { type: 'foo' };
   * events.fire(event);
   *
   * // fire event with explicit type
   * var event = { x: 10, y: 20 };
   * events.fire('element.moved', event);
   *
   * // pass additional arguments to the event
   * events.on('foo', function(event, bar) {
   *   alert(bar);
   * });
   *
   * events.fire({ type: 'foo' }, 'I am bar!');
   *
   * @param {string} [name] the optional event name
   * @param {Object} [event] the event object
   * @param {...Object} additional arguments to be passed to the callback functions
   *
   * @return {boolean} the events return value, if specified or false if the
   *                   default action was prevented by listeners
   */


  EventBus.prototype.fire = function (type, data) {
    var event, firstListener, returnValue, args;
    args = slice.call(arguments);

    if (_typeof(type) === 'object') {
      data = type;
      type = data.type;
    }

    if (!type) {
      throw new Error('no event type specified');
    }

    firstListener = this._listeners[type];

    if (!firstListener) {
      return;
    } // we make sure we fire instances of our home made
    // events here. We wrap them only once, though


    if (data instanceof InternalEvent) {
      // we are fine, we alread have an event
      event = data;
    } else {
      event = this.createEvent(data);
    } // ensure we pass the event as the first parameter


    args[0] = event; // original event type (in case we delegate)

    var originalType = event.type; // update event type before delegation

    if (type !== originalType) {
      event.type = type;
    }

    try {
      returnValue = this._invokeListeners(event, args, firstListener);
    } finally {
      // reset event type after delegation
      if (type !== originalType) {
        event.type = originalType;
      }
    } // set the return value to false if the event default
    // got prevented and no other return value exists


    if (returnValue === undefined && event.defaultPrevented) {
      returnValue = false;
    }

    return returnValue;
  };

  EventBus.prototype.handleError = function (error) {
    return this.fire('error', {
      error: error
    }) === false;
  };

  EventBus.prototype._destroy = function () {
    this._listeners = {};
  };

  EventBus.prototype._invokeListeners = function (event, args, listener) {
    var returnValue;

    while (listener) {
      // handle stopped propagation
      if (event.cancelBubble) {
        break;
      }

      returnValue = this._invokeListener(event, args, listener);
      listener = listener.next;
    }

    return returnValue;
  };

  EventBus.prototype._invokeListener = function (event, args, listener) {
    var returnValue;

    if (listener.callback.__isTomb) {
      return returnValue;
    }

    try {
      // returning false prevents the default action
      returnValue = invokeFunction(listener.callback, args); // stop propagation on return value

      if (returnValue !== undefined) {
        event.returnValue = returnValue;
        event.stopPropagation();
      } // prevent default on return false


      if (returnValue === false) {
        event.preventDefault();
      }
    } catch (e) {
      if (!this.handleError(e)) {
        console.error('unhandled error in event listener');
        console.error(e.stack);
        throw e;
      }
    }

    return returnValue;
  };
  /*
   * Add new listener with a certain priority to the list
   * of listeners (for the given event).
   *
   * The semantics of listener registration / listener execution are
   * first register, first serve: New listeners will always be inserted
   * after existing listeners with the same priority.
   *
   * Example: Inserting two listeners with priority 1000 and 1300
   *
   *    * before: [ 1500, 1500, 1000, 1000 ]
   *    * after: [ 1500, 1500, (new=1300), 1000, 1000, (new=1000) ]
   *
   * @param {string} event
   * @param {Object} listener { priority, callback }
   */


  EventBus.prototype._addListener = function (event, newListener) {
    var listener = this._getListeners(event),
        previousListener; // no prior listeners


    if (!listener) {
      this._setListeners(event, newListener);

      return;
    } // ensure we order listeners by priority from
    // 0 (high) to n > 0 (low)


    while (listener) {
      if (listener.priority < newListener.priority) {
        newListener.next = listener;

        if (previousListener) {
          previousListener.next = newListener;
        } else {
          this._setListeners(event, newListener);
        }

        return;
      }

      previousListener = listener;
      listener = listener.next;
    } // add new listener to back


    previousListener.next = newListener;
  };

  EventBus.prototype._getListeners = function (name) {
    return this._listeners[name];
  };

  EventBus.prototype._setListeners = function (name, listener) {
    this._listeners[name] = listener;
  };

  EventBus.prototype._removeListener = function (event, callback) {
    var listener = this._getListeners(event),
        nextListener,
        previousListener,
        listenerCallback;

    if (!callback) {
      // clear listeners
      this._setListeners(event, null);

      return;
    }

    while (listener) {
      nextListener = listener.next;
      listenerCallback = listener.callback;

      if (listenerCallback === callback || listenerCallback[FN_REF] === callback) {
        if (previousListener) {
          previousListener.next = nextListener;
        } else {
          // new first listener
          this._setListeners(event, nextListener);
        }
      }

      previousListener = listener;
      listener = nextListener;
    }
  };
  /**
   * A event that is emitted via the event bus.
   */


  function InternalEvent() {}

  InternalEvent.prototype.stopPropagation = function () {
    this.cancelBubble = true;
  };

  InternalEvent.prototype.preventDefault = function () {
    this.defaultPrevented = true;
  };

  InternalEvent.prototype.init = function (data) {
    assign(this, data || {});
  };
  /**
   * Invoke function. Be fast...
   *
   * @param {Function} fn
   * @param {Array<Object>} args
   *
   * @return {Any}
   */


  function invokeFunction(fn, args) {
    return fn.apply(null, args);
  }

  /**
   * Moddle base element.
   */

  function Base() {}

  Base.prototype.get = function (name) {
    return this.$model.properties.get(this, name);
  };

  Base.prototype.set = function (name, value) {
    this.$model.properties.set(this, name, value);
  };
  /**
   * A model element factory.
   *
   * @param {Moddle} model
   * @param {Properties} properties
   */


  function Factory(model, properties) {
    this.model = model;
    this.properties = properties;
  }

  Factory.prototype.createType = function (descriptor) {
    var model = this.model;
    var props = this.properties,
        prototype = Object.create(Base.prototype); // initialize default values

    forEach(descriptor.properties, function (p) {
      if (!p.isMany && p["default"] !== undefined) {
        prototype[p.name] = p["default"];
      }
    });
    props.defineModel(prototype, model);
    props.defineDescriptor(prototype, descriptor);
    var name = descriptor.ns.name;
    /**
     * The new type constructor
     */

    function ModdleElement(attrs) {
      props.define(this, '$type', {
        value: name,
        enumerable: true
      });
      props.define(this, '$attrs', {
        value: {}
      });
      props.define(this, '$parent', {
        writable: true
      });
      forEach(attrs, bind(function (val, key) {
        this.set(key, val);
      }, this));
    }

    ModdleElement.prototype = prototype;
    ModdleElement.hasType = prototype.$instanceOf = this.model.hasType; // static links

    props.defineModel(ModdleElement, model);
    props.defineDescriptor(ModdleElement, descriptor);
    return ModdleElement;
  };
  /**
   * Built-in moddle types
   */


  var BUILTINS = {
    String: true,
    Boolean: true,
    Integer: true,
    Real: true,
    Element: true
  };
  /**
   * Converters for built in types from string representations
   */

  var TYPE_CONVERTERS = {
    String: function String(s) {
      return s;
    },
    Boolean: function Boolean(s) {
      return s === 'true';
    },
    Integer: function Integer(s) {
      return parseInt(s, 10);
    },
    Real: function Real(s) {
      return parseFloat(s, 10);
    }
  };
  /**
   * Convert a type to its real representation
   */

  function coerceType(type, value) {
    var converter = TYPE_CONVERTERS[type];

    if (converter) {
      return converter(value);
    } else {
      return value;
    }
  }
  /**
   * Return whether the given type is built-in
   */


  function isBuiltIn(type) {
    return !!BUILTINS[type];
  }
  /**
   * Return whether the given type is simple
   */


  function isSimple(type) {
    return !!TYPE_CONVERTERS[type];
  }
  /**
   * Parses a namespaced attribute name of the form (ns:)localName to an object,
   * given a default prefix to assume in case no explicit namespace is given.
   *
   * @param {String} name
   * @param {String} [defaultPrefix] the default prefix to take, if none is present.
   *
   * @return {Object} the parsed name
   */


  function parseName(name, defaultPrefix) {
    var parts = name.split(/:/),
        localName,
        prefix; // no prefix (i.e. only local name)

    if (parts.length === 1) {
      localName = name;
      prefix = defaultPrefix;
    } else // prefix + local name
      if (parts.length === 2) {
        localName = parts[1];
        prefix = parts[0];
      } else {
        throw new Error('expected <prefix:localName> or <localName>, got ' + name);
      }

    name = (prefix ? prefix + ':' : '') + localName;
    return {
      name: name,
      prefix: prefix,
      localName: localName
    };
  }
  /**
   * A utility to build element descriptors.
   */


  function DescriptorBuilder(nameNs) {
    this.ns = nameNs;
    this.name = nameNs.name;
    this.allTypes = [];
    this.allTypesByName = {};
    this.properties = [];
    this.propertiesByName = {};
  }

  DescriptorBuilder.prototype.build = function () {
    return pick(this, ['ns', 'name', 'allTypes', 'allTypesByName', 'properties', 'propertiesByName', 'bodyProperty', 'idProperty']);
  };
  /**
   * Add property at given index.
   *
   * @param {Object} p
   * @param {Number} [idx]
   * @param {Boolean} [validate=true]
   */


  DescriptorBuilder.prototype.addProperty = function (p, idx, validate) {
    if (typeof idx === 'boolean') {
      validate = idx;
      idx = undefined;
    }

    this.addNamedProperty(p, validate !== false);
    var properties = this.properties;

    if (idx !== undefined) {
      properties.splice(idx, 0, p);
    } else {
      properties.push(p);
    }
  };

  DescriptorBuilder.prototype.replaceProperty = function (oldProperty, newProperty, replace) {
    var oldNameNs = oldProperty.ns;
    var props = this.properties,
        propertiesByName = this.propertiesByName,
        rename = oldProperty.name !== newProperty.name;

    if (oldProperty.isId) {
      if (!newProperty.isId) {
        throw new Error('property <' + newProperty.ns.name + '> must be id property ' + 'to refine <' + oldProperty.ns.name + '>');
      }

      this.setIdProperty(newProperty, false);
    }

    if (oldProperty.isBody) {
      if (!newProperty.isBody) {
        throw new Error('property <' + newProperty.ns.name + '> must be body property ' + 'to refine <' + oldProperty.ns.name + '>');
      } // TODO: Check compatibility


      this.setBodyProperty(newProperty, false);
    } // validate existence and get location of old property


    var idx = props.indexOf(oldProperty);

    if (idx === -1) {
      throw new Error('property <' + oldNameNs.name + '> not found in property list');
    } // remove old property


    props.splice(idx, 1); // replacing the named property is intentional
    //
    //  * validate only if this is a "rename" operation
    //  * add at specific index unless we "replace"
    //

    this.addProperty(newProperty, replace ? undefined : idx, rename); // make new property available under old name

    propertiesByName[oldNameNs.name] = propertiesByName[oldNameNs.localName] = newProperty;
  };

  DescriptorBuilder.prototype.redefineProperty = function (p, targetPropertyName, replace) {
    var nsPrefix = p.ns.prefix;
    var parts = targetPropertyName.split('#');
    var name = parseName(parts[0], nsPrefix);
    var attrName = parseName(parts[1], name.prefix).name;
    var redefinedProperty = this.propertiesByName[attrName];

    if (!redefinedProperty) {
      throw new Error('refined property <' + attrName + '> not found');
    } else {
      this.replaceProperty(redefinedProperty, p, replace);
    }

    delete p.redefines;
  };

  DescriptorBuilder.prototype.addNamedProperty = function (p, validate) {
    var ns = p.ns,
        propsByName = this.propertiesByName;

    if (validate) {
      this.assertNotDefined(p, ns.name);
      this.assertNotDefined(p, ns.localName);
    }

    propsByName[ns.name] = propsByName[ns.localName] = p;
  };

  DescriptorBuilder.prototype.removeNamedProperty = function (p) {
    var ns = p.ns,
        propsByName = this.propertiesByName;
    delete propsByName[ns.name];
    delete propsByName[ns.localName];
  };

  DescriptorBuilder.prototype.setBodyProperty = function (p, validate) {
    if (validate && this.bodyProperty) {
      throw new Error('body property defined multiple times ' + '(<' + this.bodyProperty.ns.name + '>, <' + p.ns.name + '>)');
    }

    this.bodyProperty = p;
  };

  DescriptorBuilder.prototype.setIdProperty = function (p, validate) {
    if (validate && this.idProperty) {
      throw new Error('id property defined multiple times ' + '(<' + this.idProperty.ns.name + '>, <' + p.ns.name + '>)');
    }

    this.idProperty = p;
  };

  DescriptorBuilder.prototype.assertNotDefined = function (p, name) {
    var propertyName = p.name,
        definedProperty = this.propertiesByName[propertyName];

    if (definedProperty) {
      throw new Error('property <' + propertyName + '> already defined; ' + 'override of <' + definedProperty.definedBy.ns.name + '#' + definedProperty.ns.name + '> by ' + '<' + p.definedBy.ns.name + '#' + p.ns.name + '> not allowed without redefines');
    }
  };

  DescriptorBuilder.prototype.hasProperty = function (name) {
    return this.propertiesByName[name];
  };

  DescriptorBuilder.prototype.addTrait = function (t, inherited) {
    var typesByName = this.allTypesByName,
        types = this.allTypes;
    var typeName = t.name;

    if (typeName in typesByName) {
      return;
    }

    forEach(t.properties, bind(function (p) {
      // clone property to allow extensions
      p = assign({}, p, {
        name: p.ns.localName,
        inherited: inherited
      });
      Object.defineProperty(p, 'definedBy', {
        value: t
      });
      var replaces = p.replaces,
          redefines = p.redefines; // add replace/redefine support

      if (replaces || redefines) {
        this.redefineProperty(p, replaces || redefines, replaces);
      } else {
        if (p.isBody) {
          this.setBodyProperty(p);
        }

        if (p.isId) {
          this.setIdProperty(p);
        }

        this.addProperty(p);
      }
    }, this));
    types.push(t);
    typesByName[typeName] = t;
  };
  /**
   * A registry of Moddle packages.
   *
   * @param {Array<Package>} packages
   * @param {Properties} properties
   */


  function Registry(packages, properties) {
    this.packageMap = {};
    this.typeMap = {};
    this.packages = [];
    this.properties = properties;
    forEach(packages, bind(this.registerPackage, this));
  }

  Registry.prototype.getPackage = function (uriOrPrefix) {
    return this.packageMap[uriOrPrefix];
  };

  Registry.prototype.getPackages = function () {
    return this.packages;
  };

  Registry.prototype.registerPackage = function (pkg) {
    // copy package
    pkg = assign({}, pkg);
    var pkgMap = this.packageMap;
    ensureAvailable(pkgMap, pkg, 'prefix');
    ensureAvailable(pkgMap, pkg, 'uri'); // register types

    forEach(pkg.types, bind(function (descriptor) {
      this.registerType(descriptor, pkg);
    }, this));
    pkgMap[pkg.uri] = pkgMap[pkg.prefix] = pkg;
    this.packages.push(pkg);
  };
  /**
   * Register a type from a specific package with us
   */


  Registry.prototype.registerType = function (type, pkg) {
    type = assign({}, type, {
      superClass: (type.superClass || []).slice(),
      "extends": (type["extends"] || []).slice(),
      properties: (type.properties || []).slice(),
      meta: assign(type.meta || {})
    });
    var ns = parseName(type.name, pkg.prefix),
        name = ns.name,
        propertiesByName = {}; // parse properties

    forEach(type.properties, bind(function (p) {
      // namespace property names
      var propertyNs = parseName(p.name, ns.prefix),
          propertyName = propertyNs.name; // namespace property types

      if (!isBuiltIn(p.type)) {
        p.type = parseName(p.type, propertyNs.prefix).name;
      }

      assign(p, {
        ns: propertyNs,
        name: propertyName
      });
      propertiesByName[propertyName] = p;
    }, this)); // update ns + name

    assign(type, {
      ns: ns,
      name: name,
      propertiesByName: propertiesByName
    });
    forEach(type["extends"], bind(function (extendsName) {
      var extended = this.typeMap[extendsName];
      extended.traits = extended.traits || [];
      extended.traits.push(name);
    }, this)); // link to package

    this.definePackage(type, pkg); // register

    this.typeMap[name] = type;
  };
  /**
   * Traverse the type hierarchy from bottom to top,
   * calling iterator with (type, inherited) for all elements in
   * the inheritance chain.
   *
   * @param {Object} nsName
   * @param {Function} iterator
   * @param {Boolean} [trait=false]
   */


  Registry.prototype.mapTypes = function (nsName, iterator, trait) {
    var type = isBuiltIn(nsName.name) ? {
      name: nsName.name
    } : this.typeMap[nsName.name];
    var self = this;
    /**
     * Traverse the selected trait.
     *
     * @param {String} cls
     */

    function traverseTrait(cls) {
      return traverseSuper(cls, true);
    }
    /**
     * Traverse the selected super type or trait
     *
     * @param {String} cls
     * @param {Boolean} [trait=false]
     */


    function traverseSuper(cls, trait) {
      var parentNs = parseName(cls, isBuiltIn(cls) ? '' : nsName.prefix);
      self.mapTypes(parentNs, iterator, trait);
    }

    if (!type) {
      throw new Error('unknown type <' + nsName.name + '>');
    }

    forEach(type.superClass, trait ? traverseTrait : traverseSuper); // call iterator with (type, inherited=!trait)

    iterator(type, !trait);
    forEach(type.traits, traverseTrait);
  };
  /**
   * Returns the effective descriptor for a type.
   *
   * @param  {String} type the namespaced name (ns:localName) of the type
   *
   * @return {Descriptor} the resulting effective descriptor
   */


  Registry.prototype.getEffectiveDescriptor = function (name) {
    var nsName = parseName(name);
    var builder = new DescriptorBuilder(nsName);
    this.mapTypes(nsName, function (type, inherited) {
      builder.addTrait(type, inherited);
    });
    var descriptor = builder.build(); // define package link

    this.definePackage(descriptor, descriptor.allTypes[descriptor.allTypes.length - 1].$pkg);
    return descriptor;
  };

  Registry.prototype.definePackage = function (target, pkg) {
    this.properties.define(target, '$pkg', {
      value: pkg
    });
  }; ///////// helpers ////////////////////////////


  function ensureAvailable(packageMap, pkg, identifierKey) {
    var value = pkg[identifierKey];

    if (value in packageMap) {
      throw new Error('package with ' + identifierKey + ' <' + value + '> already defined');
    }
  }
  /**
   * A utility that gets and sets properties of model elements.
   *
   * @param {Model} model
   */


  function Properties(model) {
    this.model = model;
  }
  /**
   * Sets a named property on the target element.
   * If the value is undefined, the property gets deleted.
   *
   * @param {Object} target
   * @param {String} name
   * @param {Object} value
   */


  Properties.prototype.set = function (target, name, value) {
    var property = this.model.getPropertyDescriptor(target, name);
    var propertyName = property && property.name;

    if (isUndefined$1(value)) {
      // unset the property, if the specified value is undefined;
      // delete from $attrs (for extensions) or the target itself
      if (property) {
        delete target[propertyName];
      } else {
        delete target.$attrs[name];
      }
    } else {
      // set the property, defining well defined properties on the fly
      // or simply updating them in target.$attrs (for extensions)
      if (property) {
        if (propertyName in target) {
          target[propertyName] = value;
        } else {
          defineProperty(target, property, value);
        }
      } else {
        target.$attrs[name] = value;
      }
    }
  };
  /**
   * Returns the named property of the given element
   *
   * @param  {Object} target
   * @param  {String} name
   *
   * @return {Object}
   */


  Properties.prototype.get = function (target, name) {
    var property = this.model.getPropertyDescriptor(target, name);

    if (!property) {
      return target.$attrs[name];
    }

    var propertyName = property.name; // check if access to collection property and lazily initialize it

    if (!target[propertyName] && property.isMany) {
      defineProperty(target, property, []);
    }

    return target[propertyName];
  };
  /**
   * Define a property on the target element
   *
   * @param  {Object} target
   * @param  {String} name
   * @param  {Object} options
   */


  Properties.prototype.define = function (target, name, options) {
    Object.defineProperty(target, name, options);
  };
  /**
   * Define the descriptor for an element
   */


  Properties.prototype.defineDescriptor = function (target, descriptor) {
    this.define(target, '$descriptor', {
      value: descriptor
    });
  };
  /**
   * Define the model for an element
   */


  Properties.prototype.defineModel = function (target, model) {
    this.define(target, '$model', {
      value: model
    });
  };

  function isUndefined$1(val) {
    return typeof val === 'undefined';
  }

  function defineProperty(target, property, value) {
    Object.defineProperty(target, property.name, {
      enumerable: !property.isReference,
      writable: true,
      value: value,
      configurable: true
    });
  } //// Moddle implementation /////////////////////////////////////////////////

  /**
   * @class Moddle
   *
   * A model that can be used to create elements of a specific type.
   *
   * @example
   *
   * var Moddle = require('moddle');
   *
   * var pkg = {
   *   name: 'mypackage',
   *   prefix: 'my',
   *   types: [
   *     { name: 'Root' }
   *   ]
   * };
   *
   * var moddle = new Moddle([pkg]);
   *
   * @param {Array<Package>} packages the packages to contain
   */


  function Moddle(packages) {
    this.properties = new Properties(this);
    this.factory = new Factory(this, this.properties);
    this.registry = new Registry(packages, this.properties);
    this.typeCache = {};
  }
  /**
   * Create an instance of the specified type.
   *
   * @method Moddle#create
   *
   * @example
   *
   * var foo = moddle.create('my:Foo');
   * var bar = moddle.create('my:Bar', { id: 'BAR_1' });
   *
   * @param  {String|Object} descriptor the type descriptor or name know to the model
   * @param  {Object} attrs   a number of attributes to initialize the model instance with
   * @return {Object}         model instance
   */


  Moddle.prototype.create = function (descriptor, attrs) {
    var Type = this.getType(descriptor);

    if (!Type) {
      throw new Error('unknown type <' + descriptor + '>');
    }

    return new Type(attrs);
  };
  /**
   * Returns the type representing a given descriptor
   *
   * @method Moddle#getType
   *
   * @example
   *
   * var Foo = moddle.getType('my:Foo');
   * var foo = new Foo({ 'id' : 'FOO_1' });
   *
   * @param  {String|Object} descriptor the type descriptor or name know to the model
   * @return {Object}         the type representing the descriptor
   */


  Moddle.prototype.getType = function (descriptor) {
    var cache = this.typeCache;
    var name = isString(descriptor) ? descriptor : descriptor.ns.name;
    var type = cache[name];

    if (!type) {
      descriptor = this.registry.getEffectiveDescriptor(name);
      type = cache[name] = this.factory.createType(descriptor);
    }

    return type;
  };
  /**
   * Creates an any-element type to be used within model instances.
   *
   * This can be used to create custom elements that lie outside the meta-model.
   * The created element contains all the meta-data required to serialize it
   * as part of meta-model elements.
   *
   * @method Moddle#createAny
   *
   * @example
   *
   * var foo = moddle.createAny('vendor:Foo', 'http://vendor', {
   *   value: 'bar'
   * });
   *
   * var container = moddle.create('my:Container', 'http://my', {
   *   any: [ foo ]
   * });
   *
   * // go ahead and serialize the stuff
   *
   *
   * @param  {String} name  the name of the element
   * @param  {String} nsUri the namespace uri of the element
   * @param  {Object} [properties] a map of properties to initialize the instance with
   * @return {Object} the any type instance
   */


  Moddle.prototype.createAny = function (name, nsUri, properties) {
    var nameNs = parseName(name);
    var element = {
      $type: name,
      $instanceOf: function $instanceOf(type) {
        return type === this.$type;
      }
    };
    var descriptor = {
      name: name,
      isGeneric: true,
      ns: {
        prefix: nameNs.prefix,
        localName: nameNs.localName,
        uri: nsUri
      }
    };
    this.properties.defineDescriptor(element, descriptor);
    this.properties.defineModel(element, this);
    this.properties.define(element, '$parent', {
      enumerable: false,
      writable: true
    });
    forEach(properties, function (a, key) {
      if (isObject(a) && a.value !== undefined) {
        element[a.name] = a.value;
      } else {
        element[key] = a;
      }
    });
    return element;
  };
  /**
   * Returns a registered package by uri or prefix
   *
   * @return {Object} the package
   */


  Moddle.prototype.getPackage = function (uriOrPrefix) {
    return this.registry.getPackage(uriOrPrefix);
  };
  /**
   * Returns a snapshot of all known packages
   *
   * @return {Object} the package
   */


  Moddle.prototype.getPackages = function () {
    return this.registry.getPackages();
  };
  /**
   * Returns the descriptor for an element
   */


  Moddle.prototype.getElementDescriptor = function (element) {
    return element.$descriptor;
  };
  /**
   * Returns true if the given descriptor or instance
   * represents the given type.
   *
   * May be applied to this, if element is omitted.
   */


  Moddle.prototype.hasType = function (element, type) {
    if (type === undefined) {
      type = element;
      element = this;
    }

    var descriptor = element.$model.getElementDescriptor(element);
    return type in descriptor.allTypesByName;
  };
  /**
   * Returns the descriptor of an elements named property
   */


  Moddle.prototype.getPropertyDescriptor = function (element, property) {
    return this.getElementDescriptor(element).propertiesByName[property];
  };
  /**
   * Returns a mapped type's descriptor
   */


  Moddle.prototype.getTypeDescriptor = function (type) {
    return this.registry.typeMap[type];
  };

  var fromCharCode = String.fromCharCode;
  var hasOwnProperty = Object.prototype.hasOwnProperty;
  var ENTITY_PATTERN = /&#(\d+);|&#x([0-9a-f]+);|&(\w+);/ig;
  var ENTITY_MAPPING = {
    'amp': '&',
    'apos': '\'',
    'gt': '>',
    'lt': '<',
    'quot': '"'
  }; // map UPPERCASE variants of supported special chars

  Object.keys(ENTITY_MAPPING).forEach(function (k) {
    ENTITY_MAPPING[k.toUpperCase()] = ENTITY_MAPPING[k];
  });

  function replaceEntities(_, d, x, z) {
    // reserved names, i.e. &nbsp;
    if (z) {
      if (hasOwnProperty.call(ENTITY_MAPPING, z)) {
        return ENTITY_MAPPING[z];
      } else {
        // fall back to original value
        return '&' + z + ';';
      }
    } // decimal encoded char


    if (d) {
      return fromCharCode(d);
    } // hex encoded char


    return fromCharCode(parseInt(x, 16));
  }
  /**
   * A basic entity decoder that can decode a minimal
   * sub-set of reserved names (&amp;) as well as
   * hex (&#xaaf;) and decimal (&#1231;) encoded characters.
   *
   * @param {string} str
   *
   * @return {string} decoded string
   */


  function decodeEntities(s) {
    if (s.length > 3 && s.indexOf('&') !== -1) {
      return s.replace(ENTITY_PATTERN, replaceEntities);
    }

    return s;
  }

  var XSI_URI = 'http://www.w3.org/2001/XMLSchema-instance';
  var XSI_PREFIX = 'xsi';
  var XSI_TYPE = 'xsi:type';
  var NON_WHITESPACE_OUTSIDE_ROOT_NODE = 'non-whitespace outside of root node';

  function error(msg) {
    return new Error(msg);
  }

  function missingNamespaceForPrefix(prefix) {
    return 'missing namespace for prefix <' + prefix + '>';
  }

  function getter(getFn) {
    return {
      'get': getFn,
      'enumerable': true
    };
  }

  function cloneNsMatrix(nsMatrix) {
    var clone = {},
        key;

    for (key in nsMatrix) {
      clone[key] = nsMatrix[key];
    }

    return clone;
  }

  function uriPrefix(prefix) {
    return prefix + '$uri';
  }

  function buildNsMatrix(nsUriToPrefix) {
    var nsMatrix = {},
        uri,
        prefix;

    for (uri in nsUriToPrefix) {
      prefix = nsUriToPrefix[uri];
      nsMatrix[prefix] = prefix;
      nsMatrix[uriPrefix(prefix)] = uri;
    }

    return nsMatrix;
  }

  function noopGetContext() {
    return {
      'line': 0,
      'column': 0
    };
  }

  function throwFunc(err) {
    throw err;
  }
  /**
   * Creates a new parser with the given options.
   *
   * @constructor
   *
   * @param  {!Object<string, ?>=} options
   */


  function Parser(options) {
    if (!this) {
      return new Parser(options);
    }

    var proxy = options && options['proxy'];
    var onText,
        onOpenTag,
        onCloseTag,
        onCDATA,
        onError = throwFunc,
        onWarning,
        onComment,
        onQuestion,
        onAttention;
    var getContext = noopGetContext;
    /**
     * Do we need to parse the current elements attributes for namespaces?
     *
     * @type {boolean}
     */

    var maybeNS = false;
    /**
     * Do we process namespaces at all?
     *
     * @type {boolean}
     */

    var isNamespace = false;
    /**
     * The caught error returned on parse end
     *
     * @type {Error}
     */

    var returnError = null;
    /**
     * Should we stop parsing?
     *
     * @type {boolean}
     */

    var parseStop = false;
    /**
     * A map of { uri: prefix } used by the parser.
     *
     * This map will ensure we can normalize prefixes during processing;
     * for each uri, only one prefix will be exposed to the handlers.
     *
     * @type {!Object<string, string>}}
     */

    var nsUriToPrefix;
    /**
     * Handle parse error.
     *
     * @param  {string|Error} err
     */

    function handleError(err) {
      if (!(err instanceof Error)) {
        err = error(err);
      }

      returnError = err;
      onError(err, getContext);
    }
    /**
     * Handle parse error.
     *
     * @param  {string|Error} err
     */


    function handleWarning(err) {
      if (!onWarning) {
        return;
      }

      if (!(err instanceof Error)) {
        err = error(err);
      }

      onWarning(err, getContext);
    }
    /**
     * Register parse listener.
     *
     * @param  {string}   name
     * @param  {Function} cb
     *
     * @return {Parser}
     */


    this['on'] = function (name, cb) {
      if (typeof cb !== 'function') {
        throw error('required args <name, cb>');
      }

      switch (name) {
        case 'openTag':
          onOpenTag = cb;
          break;

        case 'text':
          onText = cb;
          break;

        case 'closeTag':
          onCloseTag = cb;
          break;

        case 'error':
          onError = cb;
          break;

        case 'warn':
          onWarning = cb;
          break;

        case 'cdata':
          onCDATA = cb;
          break;

        case 'attention':
          onAttention = cb;
          break;
        // <!XXXXX zzzz="eeee">

        case 'question':
          onQuestion = cb;
          break;
        // <? ....  ?>

        case 'comment':
          onComment = cb;
          break;

        default:
          throw error('unsupported event: ' + name);
      }

      return this;
    };
    /**
     * Set the namespace to prefix mapping.
     *
     * @example
     *
     * parser.ns({
     *   'http://foo': 'foo',
     *   'http://bar': 'bar'
     * });
     *
     * @param  {!Object<string, string>} nsMap
     *
     * @return {Parser}
     */


    this['ns'] = function (nsMap) {
      if (typeof nsMap === 'undefined') {
        nsMap = {};
      }

      if (_typeof(nsMap) !== 'object') {
        throw error('required args <nsMap={}>');
      }

      var _nsUriToPrefix = {},
          k;

      for (k in nsMap) {
        _nsUriToPrefix[k] = nsMap[k];
      } // FORCE default mapping for schema instance


      _nsUriToPrefix[XSI_URI] = XSI_PREFIX;
      isNamespace = true;
      nsUriToPrefix = _nsUriToPrefix;
      return this;
    };
    /**
     * Parse xml string.
     *
     * @param  {string} xml
     *
     * @return {Error} returnError, if not thrown
     */


    this['parse'] = function (xml) {
      if (typeof xml !== 'string') {
        throw error('required args <xml=string>');
      }

      returnError = null;
      parse(xml);
      getContext = noopGetContext;
      parseStop = false;
      return returnError;
    };
    /**
     * Stop parsing.
     */


    this['stop'] = function () {
      parseStop = true;
    };
    /**
     * Parse string, invoking configured listeners on element.
     *
     * @param  {string} xml
     */


    function parse(xml) {
      var nsMatrixStack = isNamespace ? [] : null,
          nsMatrix = isNamespace ? buildNsMatrix(nsUriToPrefix) : null,
          _nsMatrix,
          nodeStack = [],
          anonymousNsCount = 0,
          tagStart = false,
          tagEnd = false,
          i = 0,
          j = 0,
          x,
          y,
          q,
          w,
          v,
          xmlns,
          elementName,
          _elementName,
          elementProxy;

      var attrsString = '',
          attrsStart = 0,
          cachedAttrs // false = parsed with errors, null = needs parsing
      ;
      /**
       * Parse attributes on demand and returns the parsed attributes.
       *
       * Return semantics: (1) `false` on attribute parse error,
       * (2) object hash on extracted attrs.
       *
       * @return {boolean|Object}
       */

      function getAttrs() {
        if (cachedAttrs !== null) {
          return cachedAttrs;
        }

        var nsUri,
            nsUriPrefix,
            nsName,
            defaultAlias = isNamespace && nsMatrix['xmlns'],
            attrList = isNamespace && maybeNS ? [] : null,
            i = attrsStart,
            s = attrsString,
            l = s.length,
            hasNewMatrix,
            newalias,
            value,
            alias,
            name,
            attrs = {},
            seenAttrs = {},
            skipAttr,
            w,
            j;

        parseAttr: for (; i < l; i++) {
          skipAttr = false;
          w = s.charCodeAt(i);

          if (w === 32 || w < 14 && w > 8) {
            // WHITESPACE={ \f\n\r\t\v}
            continue;
          } // wait for non whitespace character


          if (w < 65 || w > 122 || w > 90 && w < 97) {
            if (w !== 95 && w !== 58) {
              // char 95"_" 58":"
              handleWarning('illegal first char attribute name');
              skipAttr = true;
            }
          } // parse attribute name


          for (j = i + 1; j < l; j++) {
            w = s.charCodeAt(j);

            if (w > 96 && w < 123 || w > 64 && w < 91 || w > 47 && w < 59 || w === 46 || // '.'
            w === 45 || // '-'
            w === 95 // '_'
            ) {
                continue;
              } // unexpected whitespace


            if (w === 32 || w < 14 && w > 8) {
              // WHITESPACE
              handleWarning('missing attribute value');
              i = j;
              continue parseAttr;
            } // expected "="


            if (w === 61) {
              // "=" == 61
              break;
            }

            handleWarning('illegal attribute name char');
            skipAttr = true;
          }

          name = s.substring(i, j);

          if (name === 'xmlns:xmlns') {
            handleWarning('illegal declaration of xmlns');
            skipAttr = true;
          }

          w = s.charCodeAt(j + 1);

          if (w === 34) {
            // '"'
            j = s.indexOf('"', i = j + 2);

            if (j === -1) {
              j = s.indexOf('\'', i);

              if (j !== -1) {
                handleWarning('attribute value quote missmatch');
                skipAttr = true;
              }
            }
          } else if (w === 39) {
            // "'"
            j = s.indexOf('\'', i = j + 2);

            if (j === -1) {
              j = s.indexOf('"', i);

              if (j !== -1) {
                handleWarning('attribute value quote missmatch');
                skipAttr = true;
              }
            }
          } else {
            handleWarning('missing attribute value quotes');
            skipAttr = true; // skip to next space

            for (j = j + 1; j < l; j++) {
              w = s.charCodeAt(j + 1);

              if (w === 32 || w < 14 && w > 8) {
                // WHITESPACE
                break;
              }
            }
          }

          if (j === -1) {
            handleWarning('missing closing quotes');
            j = l;
            skipAttr = true;
          }

          if (!skipAttr) {
            value = s.substring(i, j);
          }

          i = j; // ensure SPACE follows attribute
          // skip illegal content otherwise
          // example a="b"c

          for (; j + 1 < l; j++) {
            w = s.charCodeAt(j + 1);

            if (w === 32 || w < 14 && w > 8) {
              // WHITESPACE
              break;
            } // FIRST ILLEGAL CHAR


            if (i === j) {
              handleWarning('illegal character after attribute end');
              skipAttr = true;
            }
          } // advance cursor to next attribute


          i = j + 1;

          if (skipAttr) {
            continue parseAttr;
          } // check attribute re-declaration


          if (name in seenAttrs) {
            handleWarning('attribute <' + name + '> already defined');
            continue;
          }

          seenAttrs[name] = true;

          if (!isNamespace) {
            attrs[name] = value;
            continue;
          } // try to extract namespace information


          if (maybeNS) {
            newalias = name === 'xmlns' ? 'xmlns' : name.charCodeAt(0) === 120 && name.substr(0, 6) === 'xmlns:' ? name.substr(6) : null; // handle xmlns(:alias) assignment

            if (newalias !== null) {
              nsUri = decodeEntities(value);
              nsUriPrefix = uriPrefix(newalias);
              alias = nsUriToPrefix[nsUri];

              if (!alias) {
                // no prefix defined or prefix collision
                if (newalias === 'xmlns' || nsUriPrefix in nsMatrix && nsMatrix[nsUriPrefix] !== nsUri) {
                  // alocate free ns prefix
                  do {
                    alias = 'ns' + anonymousNsCount++;
                  } while (typeof nsMatrix[alias] !== 'undefined');
                } else {
                  alias = newalias;
                }

                nsUriToPrefix[nsUri] = alias;
              }

              if (nsMatrix[newalias] !== alias) {
                if (!hasNewMatrix) {
                  nsMatrix = cloneNsMatrix(nsMatrix);
                  hasNewMatrix = true;
                }

                nsMatrix[newalias] = alias;

                if (newalias === 'xmlns') {
                  nsMatrix[uriPrefix(alias)] = nsUri;
                  defaultAlias = alias;
                }

                nsMatrix[nsUriPrefix] = nsUri;
              } // expose xmlns(:asd)="..." in attributes


              attrs[name] = value;
              continue;
            } // collect attributes until all namespace
            // declarations are processed


            attrList.push(name, value);
            continue;
          }
          /** end if (maybeNs) */
          // handle attributes on element without
          // namespace declarations


          w = name.indexOf(':');

          if (w === -1) {
            attrs[name] = value;
            continue;
          } // normalize ns attribute name


          if (!(nsName = nsMatrix[name.substring(0, w)])) {
            handleWarning(missingNamespaceForPrefix(name.substring(0, w)));
            continue;
          }

          name = defaultAlias === nsName ? name.substr(w + 1) : nsName + name.substr(w); // end: normalize ns attribute name
          // normalize xsi:type ns attribute value

          if (name === XSI_TYPE) {
            w = value.indexOf(':');

            if (w !== -1) {
              nsName = value.substring(0, w); // handle default prefixes, i.e. xs:String gracefully

              nsName = nsMatrix[nsName] || nsName;
              value = nsName + value.substring(w);
            } else {
              value = defaultAlias + ':' + value;
            }
          } // end: normalize xsi:type ns attribute value


          attrs[name] = value;
        } // handle deferred, possibly namespaced attributes


        if (maybeNS) {
          // normalize captured attributes
          for (i = 0, l = attrList.length; i < l; i++) {
            name = attrList[i++];
            value = attrList[i];
            w = name.indexOf(':');

            if (w !== -1) {
              // normalize ns attribute name
              if (!(nsName = nsMatrix[name.substring(0, w)])) {
                handleWarning(missingNamespaceForPrefix(name.substring(0, w)));
                continue;
              }

              name = defaultAlias === nsName ? name.substr(w + 1) : nsName + name.substr(w); // end: normalize ns attribute name
              // normalize xsi:type ns attribute value

              if (name === XSI_TYPE) {
                w = value.indexOf(':');

                if (w !== -1) {
                  nsName = value.substring(0, w); // handle default prefixes, i.e. xs:String gracefully

                  nsName = nsMatrix[nsName] || nsName;
                  value = nsName + value.substring(w);
                } else {
                  value = defaultAlias + ':' + value;
                }
              } // end: normalize xsi:type ns attribute value

            }

            attrs[name] = value;
          } // end: normalize captured attributes

        }

        return cachedAttrs = attrs;
      }
      /**
       * Extract the parse context { line, column, part }
       * from the current parser position.
       *
       * @return {Object} parse context
       */


      function getParseContext() {
        var splitsRe = /(\r\n|\r|\n)/g;
        var line = 0;
        var column = 0;
        var startOfLine = 0;
        var endOfLine = j;
        var match;
        var data;

        while (i >= startOfLine) {
          match = splitsRe.exec(xml);

          if (!match) {
            break;
          } // end of line = (break idx + break chars)


          endOfLine = match[0].length + match.index;

          if (endOfLine > i) {
            break;
          } // advance to next line


          line += 1;
          startOfLine = endOfLine;
        } // EOF errors


        if (i == -1) {
          column = endOfLine;
          data = xml.substring(j);
        } else // start errors
          if (j === 0) {
            data = xml.substring(j, i);
          } // other errors
          else {
              column = i - startOfLine;
              data = j == -1 ? xml.substring(i) : xml.substring(i, j + 1);
            }

        return {
          'data': data,
          'line': line,
          'column': column
        };
      }

      getContext = getParseContext;

      if (proxy) {
        elementProxy = Object.create({}, {
          'name': getter(function () {
            return elementName;
          }),
          'originalName': getter(function () {
            return _elementName;
          }),
          'attrs': getter(getAttrs),
          'ns': getter(function () {
            return nsMatrix;
          })
        });
      } // actual parse logic


      while (j !== -1) {
        if (xml.charCodeAt(j) === 60) {
          // "<"
          i = j;
        } else {
          i = xml.indexOf('<', j);
        } // parse end


        if (i === -1) {
          if (nodeStack.length) {
            return handleError('unexpected end of file');
          }

          if (j === 0) {
            return handleError('missing start tag');
          }

          if (j < xml.length) {
            if (xml.substring(j).trim()) {
              handleWarning(NON_WHITESPACE_OUTSIDE_ROOT_NODE);
            }
          }

          return;
        } // parse text


        if (j !== i) {
          if (nodeStack.length) {
            if (onText) {
              onText(xml.substring(j, i), decodeEntities, getContext);

              if (parseStop) {
                return;
              }
            }
          } else {
            if (xml.substring(j, i).trim()) {
              handleWarning(NON_WHITESPACE_OUTSIDE_ROOT_NODE);

              if (parseStop) {
                return;
              }
            }
          }
        }

        w = xml.charCodeAt(i + 1); // parse comments + CDATA

        if (w === 33) {
          // "!"
          q = xml.charCodeAt(i + 2); // CDATA section

          if (q === 91 && xml.substr(i + 3, 6) === 'CDATA[') {
            // 91 == "["
            j = xml.indexOf(']]>', i);

            if (j === -1) {
              return handleError('unclosed cdata');
            }

            if (onCDATA) {
              onCDATA(xml.substring(i + 9, j), getContext);

              if (parseStop) {
                return;
              }
            }

            j += 3;
            continue;
          } // comment


          if (q === 45 && xml.charCodeAt(i + 3) === 45) {
            // 45 == "-"
            j = xml.indexOf('-->', i);

            if (j === -1) {
              return handleError('unclosed comment');
            }

            if (onComment) {
              onComment(xml.substring(i + 4, j), decodeEntities, getContext);

              if (parseStop) {
                return;
              }
            }

            j += 3;
            continue;
          }
        } // parse question <? ... ?>


        if (w === 63) {
          // "?"
          j = xml.indexOf('?>', i);

          if (j === -1) {
            return handleError('unclosed question');
          }

          if (onQuestion) {
            onQuestion(xml.substring(i, j + 2), getContext);

            if (parseStop) {
              return;
            }
          }

          j += 2;
          continue;
        } // find matching closing tag for attention or standard tags
        // for that we must skip through attribute values
        // (enclosed in single or double quotes)


        for (x = i + 1;; x++) {
          v = xml.charCodeAt(x);

          if (isNaN(v)) {
            j = -1;
            return handleError('unclosed tag');
          } // [10] AttValue ::= '"' ([^<&"] | Reference)* '"' | "'" ([^<&'] | Reference)* "'"
          // skips the quoted string
          // (double quotes) does not appear in a literal enclosed by (double quotes)
          // (single quote) does not appear in a literal enclosed by (single quote)


          if (v === 34) {
            //  '"'
            q = xml.indexOf('"', x + 1);
            x = q !== -1 ? q : x;
          } else if (v === 39) {
            // "'"
            q = xml.indexOf("'", x + 1);
            x = q !== -1 ? q : x;
          } else if (v === 62) {
            // '>'
            j = x;
            break;
          }
        } // parse attention <! ...>
        // previously comment and CDATA have already been parsed


        if (w === 33) {
          // "!"
          if (onAttention) {
            onAttention(xml.substring(i, j + 1), decodeEntities, getContext);

            if (parseStop) {
              return;
            }
          }

          j += 1;
          continue;
        } // don't process attributes;
        // there are none


        cachedAttrs = {}; // if (xml.charCodeAt(i+1) === 47) { // </...

        if (w === 47) {
          // </...
          tagStart = false;
          tagEnd = true;

          if (!nodeStack.length) {
            return handleError('missing open tag');
          } // verify open <-> close tag match


          x = elementName = nodeStack.pop();
          q = i + 2 + x.length;

          if (xml.substring(i + 2, q) !== x) {
            return handleError('closing tag mismatch');
          } // verify chars in close tag


          for (; q < j; q++) {
            w = xml.charCodeAt(q);

            if (w === 32 || w > 8 && w < 14) {
              // \f\n\r\t\v space
              continue;
            }

            return handleError('close tag');
          }
        } else {
          if (xml.charCodeAt(j - 1) === 47) {
            // .../>
            x = elementName = xml.substring(i + 1, j - 1);
            tagStart = true;
            tagEnd = true;
          } else {
            x = elementName = xml.substring(i + 1, j);
            tagStart = true;
            tagEnd = false;
          }

          if (!(w > 96 && w < 123 || w > 64 && w < 91 || w === 95 || w === 58)) {
            // char 95"_" 58":"
            return handleError('illegal first char nodeName');
          }

          for (q = 1, y = x.length; q < y; q++) {
            w = x.charCodeAt(q);

            if (w > 96 && w < 123 || w > 64 && w < 91 || w > 47 && w < 59 || w === 45 || w === 95 || w == 46) {
              continue;
            }

            if (w === 32 || w < 14 && w > 8) {
              // \f\n\r\t\v space
              elementName = x.substring(0, q); // maybe there are attributes

              cachedAttrs = null;
              break;
            }

            return handleError('invalid nodeName');
          }

          if (!tagEnd) {
            nodeStack.push(elementName);
          }
        }

        if (isNamespace) {
          _nsMatrix = nsMatrix;

          if (tagStart) {
            // remember old namespace
            // unless we're self-closing
            if (!tagEnd) {
              nsMatrixStack.push(_nsMatrix);
            }

            if (cachedAttrs === null) {
              // quick check, whether there may be namespace
              // declarations on the node; if that is the case
              // we need to eagerly parse the node attributes
              if (maybeNS = x.indexOf('xmlns', q) !== -1) {
                attrsStart = q;
                attrsString = x;
                getAttrs();
                maybeNS = false;
              }
            }
          }

          _elementName = elementName;
          w = elementName.indexOf(':');

          if (w !== -1) {
            xmlns = nsMatrix[elementName.substring(0, w)]; // prefix given; namespace must exist

            if (!xmlns) {
              return handleError('missing namespace on <' + _elementName + '>');
            }

            elementName = elementName.substr(w + 1);
          } else {
            xmlns = nsMatrix['xmlns']; // if no default namespace is defined,
            // we'll import the element as anonymous.
            //
            // it is up to users to correct that to the document defined
            // targetNamespace, or whatever their undersanding of the
            // XML spec mandates.
          } // adjust namespace prefixs as configured


          if (xmlns) {
            elementName = xmlns + ':' + elementName;
          }
        }

        if (tagStart) {
          attrsStart = q;
          attrsString = x;

          if (onOpenTag) {
            if (proxy) {
              onOpenTag(elementProxy, decodeEntities, tagEnd, getContext);
            } else {
              onOpenTag(elementName, getAttrs, decodeEntities, tagEnd, getContext);
            }

            if (parseStop) {
              return;
            }
          }
        }

        if (tagEnd) {
          if (onCloseTag) {
            onCloseTag(proxy ? elementProxy : elementName, decodeEntities, tagStart, getContext);

            if (parseStop) {
              return;
            }
          } // restore old namespace


          if (isNamespace) {
            if (!tagStart) {
              nsMatrix = nsMatrixStack.pop();
            } else {
              nsMatrix = _nsMatrix;
            }
          }
        }

        j += 1;
      }
    }
    /** end parse */

  }

  function hasLowerCaseAlias(pkg) {
    return pkg.xml && pkg.xml.tagAlias === 'lowerCase';
  }

  var DEFAULT_NS_MAP = {
    'xsi': 'http://www.w3.org/2001/XMLSchema-instance',
    'xml': 'http://www.w3.org/XML/1998/namespace'
  };
  var XSI_TYPE$1 = 'xsi:type';

  function serializeFormat(element) {
    return element.xml && element.xml.serialize;
  }

  function serializeAsType(element) {
    return serializeFormat(element) === XSI_TYPE$1;
  }

  function serializeAsProperty(element) {
    return serializeFormat(element) === 'property';
  }

  function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
  }

  function aliasToName(aliasNs, pkg) {
    if (!hasLowerCaseAlias(pkg)) {
      return aliasNs.name;
    }

    return aliasNs.prefix + ':' + capitalize(aliasNs.localName);
  }

  function prefixedToName(nameNs, pkg) {
    var name = nameNs.name,
        localName = nameNs.localName;
    var typePrefix = pkg.xml && pkg.xml.typePrefix;

    if (typePrefix && localName.indexOf(typePrefix) === 0) {
      return nameNs.prefix + ':' + localName.slice(typePrefix.length);
    } else {
      return name;
    }
  }

  function normalizeXsiTypeName(name, model) {
    var nameNs = parseName(name);
    var pkg = model.getPackage(nameNs.prefix);
    return prefixedToName(nameNs, pkg);
  }

  function error$1(message) {
    return new Error(message);
  }
  /**
   * Get the moddle descriptor for a given instance or type.
   *
   * @param  {ModdleElement|Function} element
   *
   * @return {Object} the moddle descriptor
   */


  function getModdleDescriptor(element) {
    return element.$descriptor;
  }

  function defer(fn) {
    setTimeout(fn, 0);
  }
  /**
   * A parse context.
   *
   * @class
   *
   * @param {Object} options
   * @param {ElementHandler} options.rootHandler the root handler for parsing a document
   * @param {boolean} [options.lax=false] whether or not to ignore invalid elements
   */


  function Context(options) {
    /**
     * @property {ElementHandler} rootHandler
     */

    /**
     * @property {Boolean} lax
     */
    assign(this, options);
    this.elementsById = {};
    this.references = [];
    this.warnings = [];
    /**
     * Add an unresolved reference.
     *
     * @param {Object} reference
     */

    this.addReference = function (reference) {
      this.references.push(reference);
    };
    /**
     * Add a processed element.
     *
     * @param {ModdleElement} element
     */


    this.addElement = function (element) {
      if (!element) {
        throw error$1('expected element');
      }

      var elementsById = this.elementsById;
      var descriptor = getModdleDescriptor(element);
      var idProperty = descriptor.idProperty,
          id;

      if (idProperty) {
        id = element.get(idProperty.name);

        if (id) {
          // for QName validation as per http://www.w3.org/TR/REC-xml/#NT-NameChar
          if (!/^([a-z][\w-.]*:)?[a-z_][\w-.]*$/i.test(id)) {
            throw new Error('illegal ID <' + id + '>');
          }

          if (elementsById[id]) {
            throw error$1('duplicate ID <' + id + '>');
          }

          elementsById[id] = element;
        }
      }
    };
    /**
     * Add an import warning.
     *
     * @param {Object} warning
     * @param {String} warning.message
     * @param {Error} [warning.error]
     */


    this.addWarning = function (warning) {
      this.warnings.push(warning);
    };
  }

  function BaseHandler() {}

  BaseHandler.prototype.handleEnd = function () {};

  BaseHandler.prototype.handleText = function () {};

  BaseHandler.prototype.handleNode = function () {};
  /**
   * A simple pass through handler that does nothing except for
   * ignoring all input it receives.
   *
   * This is used to ignore unknown elements and
   * attributes.
   */


  function NoopHandler() {}

  NoopHandler.prototype = Object.create(BaseHandler.prototype);

  NoopHandler.prototype.handleNode = function () {
    return this;
  };

  function BodyHandler() {}

  BodyHandler.prototype = Object.create(BaseHandler.prototype);

  BodyHandler.prototype.handleText = function (text) {
    this.body = (this.body || '') + text;
  };

  function ReferenceHandler(property, context) {
    this.property = property;
    this.context = context;
  }

  ReferenceHandler.prototype = Object.create(BodyHandler.prototype);

  ReferenceHandler.prototype.handleNode = function (node) {
    if (this.element) {
      throw error$1('expected no sub nodes');
    } else {
      this.element = this.createReference(node);
    }

    return this;
  };

  ReferenceHandler.prototype.handleEnd = function () {
    this.element.id = this.body;
  };

  ReferenceHandler.prototype.createReference = function (node) {
    return {
      property: this.property.ns.name,
      id: ''
    };
  };

  function ValueHandler(propertyDesc, element) {
    this.element = element;
    this.propertyDesc = propertyDesc;
  }

  ValueHandler.prototype = Object.create(BodyHandler.prototype);

  ValueHandler.prototype.handleEnd = function () {
    var value = this.body || '',
        element = this.element,
        propertyDesc = this.propertyDesc;
    value = coerceType(propertyDesc.type, value);

    if (propertyDesc.isMany) {
      element.get(propertyDesc.name).push(value);
    } else {
      element.set(propertyDesc.name, value);
    }
  };

  function BaseElementHandler() {}

  BaseElementHandler.prototype = Object.create(BodyHandler.prototype);

  BaseElementHandler.prototype.handleNode = function (node) {
    var parser = this,
        element = this.element;

    if (!element) {
      element = this.element = this.createElement(node);
      this.context.addElement(element);
    } else {
      parser = this.handleChild(node);
    }

    return parser;
  };
  /**
   * @class Reader.ElementHandler
   *
   */


  function ElementHandler(model, typeName, context) {
    this.model = model;
    this.type = model.getType(typeName);
    this.context = context;
  }

  ElementHandler.prototype = Object.create(BaseElementHandler.prototype);

  ElementHandler.prototype.addReference = function (reference) {
    this.context.addReference(reference);
  };

  ElementHandler.prototype.handleText = function (text) {
    var element = this.element,
        descriptor = getModdleDescriptor(element),
        bodyProperty = descriptor.bodyProperty;

    if (!bodyProperty) {
      throw error$1('unexpected body text <' + text + '>');
    }

    BodyHandler.prototype.handleText.call(this, text);
  };

  ElementHandler.prototype.handleEnd = function () {
    var value = this.body,
        element = this.element,
        descriptor = getModdleDescriptor(element),
        bodyProperty = descriptor.bodyProperty;

    if (bodyProperty && value !== undefined) {
      value = coerceType(bodyProperty.type, value);
      element.set(bodyProperty.name, value);
    }
  };
  /**
   * Create an instance of the model from the given node.
   *
   * @param  {Element} node the xml node
   */


  ElementHandler.prototype.createElement = function (node) {
    var attributes = node.attributes,
        Type = this.type,
        descriptor = getModdleDescriptor(Type),
        context = this.context,
        instance = new Type({}),
        model = this.model,
        propNameNs;
    forEach(attributes, function (value, name) {
      var prop = descriptor.propertiesByName[name],
          values;

      if (prop && prop.isReference) {
        if (!prop.isMany) {
          context.addReference({
            element: instance,
            property: prop.ns.name,
            id: value
          });
        } else {
          // IDREFS: parse references as whitespace-separated list
          values = value.split(' ');
          forEach(values, function (v) {
            context.addReference({
              element: instance,
              property: prop.ns.name,
              id: v
            });
          });
        }
      } else {
        if (prop) {
          value = coerceType(prop.type, value);
        } else if (name !== 'xmlns') {
          propNameNs = parseName(name, descriptor.ns.prefix); // check whether attribute is defined in a well-known namespace
          // if that is the case we emit a warning to indicate potential misuse

          if (model.getPackage(propNameNs.prefix)) {
            context.addWarning({
              message: 'unknown attribute <' + name + '>',
              element: instance,
              property: name,
              value: value
            });
          }
        }

        instance.set(name, value);
      }
    });
    return instance;
  };

  ElementHandler.prototype.getPropertyForNode = function (node) {
    var name = node.name;
    var nameNs = parseName(name);
    var type = this.type,
        model = this.model,
        descriptor = getModdleDescriptor(type);
    var propertyName = nameNs.name,
        property = descriptor.propertiesByName[propertyName],
        elementTypeName,
        elementType; // search for properties by name first

    if (property && !property.isAttr) {
      if (serializeAsType(property)) {
        elementTypeName = node.attributes[XSI_TYPE$1]; // xsi type is optional, if it does not exists the
        // default type is assumed

        if (elementTypeName) {
          // take possible type prefixes from XML
          // into account, i.e.: xsi:type="t{ActualType}"
          elementTypeName = normalizeXsiTypeName(elementTypeName, model);
          elementType = model.getType(elementTypeName);
          return assign({}, property, {
            effectiveType: getModdleDescriptor(elementType).name
          });
        }
      } // search for properties by name first


      return property;
    }

    var pkg = model.getPackage(nameNs.prefix);

    if (pkg) {
      elementTypeName = aliasToName(nameNs, pkg);
      elementType = model.getType(elementTypeName); // search for collection members later

      property = find(descriptor.properties, function (p) {
        return !p.isVirtual && !p.isReference && !p.isAttribute && elementType.hasType(p.type);
      });

      if (property) {
        return assign({}, property, {
          effectiveType: getModdleDescriptor(elementType).name
        });
      }
    } else {
      // parse unknown element (maybe extension)
      property = find(descriptor.properties, function (p) {
        return !p.isReference && !p.isAttribute && p.type === 'Element';
      });

      if (property) {
        return property;
      }
    }

    throw error$1('unrecognized element <' + nameNs.name + '>');
  };

  ElementHandler.prototype.toString = function () {
    return 'ElementDescriptor[' + getModdleDescriptor(this.type).name + ']';
  };

  ElementHandler.prototype.valueHandler = function (propertyDesc, element) {
    return new ValueHandler(propertyDesc, element);
  };

  ElementHandler.prototype.referenceHandler = function (propertyDesc) {
    return new ReferenceHandler(propertyDesc, this.context);
  };

  ElementHandler.prototype.handler = function (type) {
    if (type === 'Element') {
      return new GenericElementHandler(this.model, type, this.context);
    } else {
      return new ElementHandler(this.model, type, this.context);
    }
  };
  /**
   * Handle the child element parsing
   *
   * @param  {Element} node the xml node
   */


  ElementHandler.prototype.handleChild = function (node) {
    var propertyDesc, type, element, childHandler;
    propertyDesc = this.getPropertyForNode(node);
    element = this.element;
    type = propertyDesc.effectiveType || propertyDesc.type;

    if (isSimple(type)) {
      return this.valueHandler(propertyDesc, element);
    }

    if (propertyDesc.isReference) {
      childHandler = this.referenceHandler(propertyDesc).handleNode(node);
    } else {
      childHandler = this.handler(type).handleNode(node);
    }

    var newElement = childHandler.element; // child handles may decide to skip elements
    // by not returning anything

    if (newElement !== undefined) {
      if (propertyDesc.isMany) {
        element.get(propertyDesc.name).push(newElement);
      } else {
        element.set(propertyDesc.name, newElement);
      }

      if (propertyDesc.isReference) {
        assign(newElement, {
          element: element
        });
        this.context.addReference(newElement);
      } else {
        // establish child -> parent relationship
        newElement.$parent = element;
      }
    }

    return childHandler;
  };
  /**
   * An element handler that performs special validation
   * to ensure the node it gets initialized with matches
   * the handlers type (namespace wise).
   *
   * @param {Moddle} model
   * @param {String} typeName
   * @param {Context} context
   */


  function RootElementHandler(model, typeName, context) {
    ElementHandler.call(this, model, typeName, context);
  }

  RootElementHandler.prototype = Object.create(ElementHandler.prototype);

  RootElementHandler.prototype.createElement = function (node) {
    var name = node.name,
        nameNs = parseName(name),
        model = this.model,
        type = this.type,
        pkg = model.getPackage(nameNs.prefix),
        typeName = pkg && aliasToName(nameNs, pkg) || name; // verify the correct namespace if we parse
    // the first element in the handler tree
    //
    // this ensures we don't mistakenly import wrong namespace elements

    if (!type.hasType(typeName)) {
      throw error$1('unexpected element <' + node.originalName + '>');
    }

    return ElementHandler.prototype.createElement.call(this, node);
  };

  function GenericElementHandler(model, typeName, context) {
    this.model = model;
    this.context = context;
  }

  GenericElementHandler.prototype = Object.create(BaseElementHandler.prototype);

  GenericElementHandler.prototype.createElement = function (node) {
    var name = node.name,
        ns = parseName(name),
        prefix = ns.prefix,
        uri = node.ns[prefix + '$uri'],
        attributes = node.attributes;
    return this.model.createAny(name, uri, attributes);
  };

  GenericElementHandler.prototype.handleChild = function (node) {
    var handler = new GenericElementHandler(this.model, 'Element', this.context).handleNode(node),
        element = this.element;
    var newElement = handler.element,
        children;

    if (newElement !== undefined) {
      children = element.$children = element.$children || [];
      children.push(newElement); // establish child -> parent relationship

      newElement.$parent = element;
    }

    return handler;
  };

  GenericElementHandler.prototype.handleEnd = function () {
    if (this.body) {
      this.element.$body = this.body;
    }
  };
  /**
   * A reader for a meta-model
   *
   * @param {Object} options
   * @param {Model} options.model used to read xml files
   * @param {Boolean} options.lax whether to make parse errors warnings
   */


  function Reader(options) {
    if (options instanceof Moddle) {
      options = {
        model: options
      };
    }

    assign(this, {
      lax: false
    }, options);
  }
  /**
   * Parse the given XML into a moddle document tree.
   *
   * @param {String} xml
   * @param {ElementHandler|Object} options or rootHandler
   * @param  {Function} done
   */


  Reader.prototype.fromXML = function (xml, options, done) {
    var rootHandler = options.rootHandler;

    if (options instanceof ElementHandler) {
      // root handler passed via (xml, { rootHandler: ElementHandler }, ...)
      rootHandler = options;
      options = {};
    } else {
      if (typeof options === 'string') {
        // rootHandler passed via (xml, 'someString', ...)
        rootHandler = this.handler(options);
        options = {};
      } else if (typeof rootHandler === 'string') {
        // rootHandler passed via (xml, { rootHandler: 'someString' }, ...)
        rootHandler = this.handler(rootHandler);
      }
    }

    var model = this.model,
        lax = this.lax;
    var context = new Context(assign({}, options, {
      rootHandler: rootHandler
    })),
        parser = new Parser({
      proxy: true
    }),
        stack = createStack();
    rootHandler.context = context; // push root handler

    stack.push(rootHandler);
    /**
     * Handle error.
     *
     * @param  {Error} err
     * @param  {Function} getContext
     * @param  {boolean} lax
     *
     * @return {boolean} true if handled
     */

    function handleError(err, getContext, lax) {
      var ctx = getContext();
      var line = ctx.line,
          column = ctx.column,
          data = ctx.data; // we receive the full context data here,
      // for elements trim down the information
      // to the tag name, only

      if (data.charAt(0) === '<' && data.indexOf(' ') !== -1) {
        data = data.slice(0, data.indexOf(' ')) + '>';
      }

      var message = 'unparsable content ' + (data ? data + ' ' : '') + 'detected\n\t' + 'line: ' + line + '\n\t' + 'column: ' + column + '\n\t' + 'nested error: ' + err.message;

      if (lax) {
        context.addWarning({
          message: message,
          error: err
        });
        return true;
      } else {
        throw error$1(message);
      }
    }

    function handleWarning(err, getContext) {
      // just like handling errors in <lax=true> mode
      return handleError(err, getContext, true);
    }
    /**
     * Resolve collected references on parse end.
     */


    function resolveReferences() {
      var elementsById = context.elementsById;
      var references = context.references;
      var i, r;

      for (i = 0; r = references[i]; i++) {
        var element = r.element;
        var reference = elementsById[r.id];
        var property = getModdleDescriptor(element).propertiesByName[r.property];

        if (!reference) {
          context.addWarning({
            message: 'unresolved reference <' + r.id + '>',
            element: r.element,
            property: r.property,
            value: r.id
          });
        }

        if (property.isMany) {
          var collection = element.get(property.name),
              idx = collection.indexOf(r); // we replace an existing place holder (idx != -1) or
          // append to the collection instead

          if (idx === -1) {
            idx = collection.length;
          }

          if (!reference) {
            // remove unresolvable reference
            collection.splice(idx, 1);
          } else {
            // add or update reference in collection
            collection[idx] = reference;
          }
        } else {
          element.set(property.name, reference);
        }
      }
    }

    function handleClose() {
      stack.pop().handleEnd();
    }

    var PREAMBLE_START_PATTERN = /^<\?xml /i;
    var ENCODING_PATTERN = / encoding="([^"]+)"/i;
    var UTF_8_PATTERN = /^utf-8$/i;

    function handleQuestion(question) {
      if (!PREAMBLE_START_PATTERN.test(question)) {
        return;
      }

      var match = ENCODING_PATTERN.exec(question);
      var encoding = match && match[1];

      if (!encoding || UTF_8_PATTERN.test(encoding)) {
        return;
      }

      context.addWarning({
        message: 'unsupported document encoding <' + encoding + '>, ' + 'falling back to UTF-8'
      });
    }

    function handleOpen(node, getContext) {
      var handler = stack.peek();

      try {
        stack.push(handler.handleNode(node));
      } catch (err) {
        if (handleError(err, getContext, lax)) {
          stack.push(new NoopHandler());
        }
      }
    }

    function handleCData(text, getContext) {
      try {
        stack.peek().handleText(text);
      } catch (err) {
        handleWarning(err, getContext);
      }
    }

    function handleText(text, getContext) {
      // strip whitespace only nodes, i.e. before
      // <!CDATA[ ... ]> sections and in between tags
      text = text.trim();

      if (!text) {
        return;
      }

      handleCData(text, getContext);
    }

    var uriMap = model.getPackages().reduce(function (uriMap, p) {
      uriMap[p.uri] = p.prefix;
      return uriMap;
    }, {
      'http://www.w3.org/XML/1998/namespace': 'xml' // add default xml ns

    });
    parser.ns(uriMap).on('openTag', function (obj, decodeStr, selfClosing, getContext) {
      // gracefully handle unparsable attributes (attrs=false)
      var attrs = obj.attrs || {};
      var decodedAttrs = Object.keys(attrs).reduce(function (d, key) {
        var value = decodeStr(attrs[key]);
        d[key] = value;
        return d;
      }, {});
      var node = {
        name: obj.name,
        originalName: obj.originalName,
        attributes: decodedAttrs,
        ns: obj.ns
      };
      handleOpen(node, getContext);
    }).on('question', handleQuestion).on('closeTag', handleClose).on('cdata', handleCData).on('text', function (text, decodeEntities, getContext) {
      handleText(decodeEntities(text), getContext);
    }).on('error', handleError).on('warn', handleWarning); // deferred parse XML to make loading really ascnchronous
    // this ensures the execution environment (node or browser)
    // is kept responsive and that certain optimization strategies
    // can kick in

    defer(function () {
      var err;

      try {
        parser.parse(xml);
        resolveReferences();
      } catch (e) {
        err = e;
      }

      var element = rootHandler.element; // handle the situation that we could not extract
      // the desired root element from the document

      if (!err && !element) {
        err = error$1('failed to parse document as <' + rootHandler.type.$descriptor.name + '>');
      }

      done(err, err ? undefined : element, context);
    });
  };

  Reader.prototype.handler = function (name) {
    return new RootElementHandler(this.model, name);
  }; // helpers //////////////////////////


  function createStack() {
    var stack = [];
    Object.defineProperty(stack, 'peek', {
      value: function value() {
        return this[this.length - 1];
      }
    });
    return stack;
  }

  var XML_PREAMBLE = '<?xml version="1.0" encoding="UTF-8"?>\n';
  var ESCAPE_ATTR_CHARS = /<|>|'|"|&|\n\r|\n/g;
  var ESCAPE_CHARS = /<|>|&/g;

  function Namespaces(parent) {
    var prefixMap = {};
    var uriMap = {};
    var used = {};
    var wellknown = [];
    var custom = []; // API

    this.byUri = function (uri) {
      return uriMap[uri] || parent && parent.byUri(uri);
    };

    this.add = function (ns, isWellknown) {
      uriMap[ns.uri] = ns;

      if (isWellknown) {
        wellknown.push(ns);
      } else {
        custom.push(ns);
      }

      this.mapPrefix(ns.prefix, ns.uri);
    };

    this.uriByPrefix = function (prefix) {
      return prefixMap[prefix || 'xmlns'];
    };

    this.mapPrefix = function (prefix, uri) {
      prefixMap[prefix || 'xmlns'] = uri;
    };

    this.getNSKey = function (ns) {
      return ns.prefix !== undefined ? ns.uri + '|' + ns.prefix : ns.uri;
    };

    this.logUsed = function (ns) {
      var uri = ns.uri;
      var nsKey = this.getNSKey(ns);
      used[nsKey] = this.byUri(uri); // Inform parent recursively about the usage of this NS

      if (parent) {
        parent.logUsed(ns);
      }
    };

    this.getUsed = function (ns) {
      function isUsed(ns) {
        var nsKey = self.getNSKey(ns);
        return used[nsKey];
      }

      var self = this;
      var allNs = [].concat(wellknown, custom);
      return allNs.filter(isUsed);
    };
  }

  function lower(string) {
    return string.charAt(0).toLowerCase() + string.slice(1);
  }

  function nameToAlias(name, pkg) {
    if (hasLowerCaseAlias(pkg)) {
      return lower(name);
    } else {
      return name;
    }
  }

  function inherits(ctor, superCtor) {
    ctor.super_ = superCtor;
    ctor.prototype = Object.create(superCtor.prototype, {
      constructor: {
        value: ctor,
        enumerable: false,
        writable: true,
        configurable: true
      }
    });
  }

  function nsName(ns) {
    if (isString(ns)) {
      return ns;
    } else {
      return (ns.prefix ? ns.prefix + ':' : '') + ns.localName;
    }
  }

  function getNsAttrs(namespaces) {
    return map(namespaces.getUsed(), function (ns) {
      var name = 'xmlns' + (ns.prefix ? ':' + ns.prefix : '');
      return {
        name: name,
        value: ns.uri
      };
    });
  }

  function getElementNs(ns, descriptor) {
    if (descriptor.isGeneric) {
      return assign({
        localName: descriptor.ns.localName
      }, ns);
    } else {
      return assign({
        localName: nameToAlias(descriptor.ns.localName, descriptor.$pkg)
      }, ns);
    }
  }

  function getPropertyNs(ns, descriptor) {
    return assign({
      localName: descriptor.ns.localName
    }, ns);
  }

  function getSerializableProperties(element) {
    var descriptor = element.$descriptor;
    return filter(descriptor.properties, function (p) {
      var name = p.name;

      if (p.isVirtual) {
        return false;
      } // do not serialize defaults


      if (!element.hasOwnProperty(name)) {
        return false;
      }

      var value = element[name]; // do not serialize default equals

      if (value === p["default"]) {
        return false;
      } // do not serialize null properties


      if (value === null) {
        return false;
      }

      return p.isMany ? value.length : true;
    });
  }

  var ESCAPE_ATTR_MAP = {
    '\n': '#10',
    '\n\r': '#10',
    '"': '#34',
    '\'': '#39',
    '<': '#60',
    '>': '#62',
    '&': '#38'
  };
  var ESCAPE_MAP = {
    '<': 'lt',
    '>': 'gt',
    '&': 'amp'
  };

  function escape(str, charPattern, replaceMap) {
    // ensure we are handling strings here
    str = isString(str) ? str : '' + str;
    return str.replace(charPattern, function (s) {
      return '&' + replaceMap[s] + ';';
    });
  }
  /**
   * Escape a string attribute to not contain any bad values (line breaks, '"', ...)
   *
   * @param {String} str the string to escape
   * @return {String} the escaped string
   */


  function escapeAttr(str) {
    return escape(str, ESCAPE_ATTR_CHARS, ESCAPE_ATTR_MAP);
  }

  function escapeBody(str) {
    return escape(str, ESCAPE_CHARS, ESCAPE_MAP);
  }

  function filterAttributes(props) {
    return filter(props, function (p) {
      return p.isAttr;
    });
  }

  function filterContained(props) {
    return filter(props, function (p) {
      return !p.isAttr;
    });
  }

  function ReferenceSerializer(tagName) {
    this.tagName = tagName;
  }

  ReferenceSerializer.prototype.build = function (element) {
    this.element = element;
    return this;
  };

  ReferenceSerializer.prototype.serializeTo = function (writer) {
    writer.appendIndent().append('<' + this.tagName + '>' + this.element.id + '</' + this.tagName + '>').appendNewLine();
  };

  function BodySerializer() {}

  BodySerializer.prototype.serializeValue = BodySerializer.prototype.serializeTo = function (writer) {
    writer.append(this.escape ? escapeBody(this.value) : this.value);
  };

  BodySerializer.prototype.build = function (prop, value) {
    this.value = value;

    if (prop.type === 'String' && value.search(ESCAPE_CHARS) !== -1) {
      this.escape = true;
    }

    return this;
  };

  function ValueSerializer(tagName) {
    this.tagName = tagName;
  }

  inherits(ValueSerializer, BodySerializer);

  ValueSerializer.prototype.serializeTo = function (writer) {
    writer.appendIndent().append('<' + this.tagName + '>');
    this.serializeValue(writer);
    writer.append('</' + this.tagName + '>').appendNewLine();
  };

  function ElementSerializer(parent, propertyDescriptor) {
    this.body = [];
    this.attrs = [];
    this.parent = parent;
    this.propertyDescriptor = propertyDescriptor;
  }

  ElementSerializer.prototype.build = function (element) {
    this.element = element;
    var elementDescriptor = element.$descriptor,
        propertyDescriptor = this.propertyDescriptor;
    var otherAttrs, properties;
    var isGeneric = elementDescriptor.isGeneric;

    if (isGeneric) {
      otherAttrs = this.parseGeneric(element);
    } else {
      otherAttrs = this.parseNsAttributes(element);
    }

    if (propertyDescriptor) {
      this.ns = this.nsPropertyTagName(propertyDescriptor);
    } else {
      this.ns = this.nsTagName(elementDescriptor);
    } // compute tag name


    this.tagName = this.addTagName(this.ns);

    if (!isGeneric) {
      properties = getSerializableProperties(element);
      this.parseAttributes(filterAttributes(properties));
      this.parseContainments(filterContained(properties));
    }

    this.parseGenericAttributes(element, otherAttrs);
    return this;
  };

  ElementSerializer.prototype.nsTagName = function (descriptor) {
    var effectiveNs = this.logNamespaceUsed(descriptor.ns);
    return getElementNs(effectiveNs, descriptor);
  };

  ElementSerializer.prototype.nsPropertyTagName = function (descriptor) {
    var effectiveNs = this.logNamespaceUsed(descriptor.ns);
    return getPropertyNs(effectiveNs, descriptor);
  };

  ElementSerializer.prototype.isLocalNs = function (ns) {
    return ns.uri === this.ns.uri;
  };
  /**
   * Get the actual ns attribute name for the given element.
   *
   * @param {Object} element
   * @param {Boolean} [element.inherited=false]
   *
   * @return {Object} nsName
   */


  ElementSerializer.prototype.nsAttributeName = function (element) {
    var ns;

    if (isString(element)) {
      ns = parseName(element);
    } else {
      ns = element.ns;
    } // return just local name for inherited attributes


    if (element.inherited) {
      return {
        localName: ns.localName
      };
    } // parse + log effective ns


    var effectiveNs = this.logNamespaceUsed(ns); // LOG ACTUAL namespace use

    this.getNamespaces().logUsed(effectiveNs); // strip prefix if same namespace like parent

    if (this.isLocalNs(effectiveNs)) {
      return {
        localName: ns.localName
      };
    } else {
      return assign({
        localName: ns.localName
      }, effectiveNs);
    }
  };

  ElementSerializer.prototype.parseGeneric = function (element) {
    var self = this,
        body = this.body;
    var attributes = [];
    forEach(element, function (val, key) {
      var nonNsAttr;

      if (key === '$body') {
        body.push(new BodySerializer().build({
          type: 'String'
        }, val));
      } else if (key === '$children') {
        forEach(val, function (child) {
          body.push(new ElementSerializer(self).build(child));
        });
      } else if (key.indexOf('$') !== 0) {
        nonNsAttr = self.parseNsAttribute(element, key, val);

        if (nonNsAttr) {
          attributes.push({
            name: key,
            value: val
          });
        }
      }
    });
    return attributes;
  };

  ElementSerializer.prototype.parseNsAttribute = function (element, name, value) {
    var model = element.$model;
    var nameNs = parseName(name);
    var ns; // parse xmlns:foo="http://foo.bar"

    if (nameNs.prefix === 'xmlns') {
      ns = {
        prefix: nameNs.localName,
        uri: value
      };
    } // parse xmlns="http://foo.bar"


    if (!nameNs.prefix && nameNs.localName === 'xmlns') {
      ns = {
        uri: value
      };
    }

    if (!ns) {
      return {
        name: name,
        value: value
      };
    }

    if (model && model.getPackage(value)) {
      // register well known namespace
      this.logNamespace(ns, true, true);
    } else {
      // log custom namespace directly as used
      var actualNs = this.logNamespaceUsed(ns, true);
      this.getNamespaces().logUsed(actualNs);
    }
  };
  /**
   * Parse namespaces and return a list of left over generic attributes
   *
   * @param  {Object} element
   * @return {Array<Object>}
   */


  ElementSerializer.prototype.parseNsAttributes = function (element, attrs) {
    var self = this;
    var genericAttrs = element.$attrs;
    var attributes = []; // parse namespace attributes first
    // and log them. push non namespace attributes to a list
    // and process them later

    forEach(genericAttrs, function (value, name) {
      var nonNsAttr = self.parseNsAttribute(element, name, value);

      if (nonNsAttr) {
        attributes.push(nonNsAttr);
      }
    });
    return attributes;
  };

  ElementSerializer.prototype.parseGenericAttributes = function (element, attributes) {
    var self = this;
    forEach(attributes, function (attr) {
      // do not serialize xsi:type attribute
      // it is set manually based on the actual implementation type
      if (attr.name === XSI_TYPE$1) {
        return;
      }

      try {
        self.addAttribute(self.nsAttributeName(attr.name), attr.value);
      } catch (e) {
        console.warn('missing namespace information for ', attr.name, '=', attr.value, 'on', element, e);
      }
    });
  };

  ElementSerializer.prototype.parseContainments = function (properties) {
    var self = this,
        body = this.body,
        element = this.element;
    forEach(properties, function (p) {
      var value = element.get(p.name),
          isReference = p.isReference,
          isMany = p.isMany;

      if (!isMany) {
        value = [value];
      }

      if (p.isBody) {
        body.push(new BodySerializer().build(p, value[0]));
      } else if (isSimple(p.type)) {
        forEach(value, function (v) {
          body.push(new ValueSerializer(self.addTagName(self.nsPropertyTagName(p))).build(p, v));
        });
      } else if (isReference) {
        forEach(value, function (v) {
          body.push(new ReferenceSerializer(self.addTagName(self.nsPropertyTagName(p))).build(v));
        });
      } else {
        // allow serialization via type
        // rather than element name
        var asType = serializeAsType(p),
            asProperty = serializeAsProperty(p);
        forEach(value, function (v) {
          var serializer;

          if (asType) {
            serializer = new TypeSerializer(self, p);
          } else if (asProperty) {
            serializer = new ElementSerializer(self, p);
          } else {
            serializer = new ElementSerializer(self);
          }

          body.push(serializer.build(v));
        });
      }
    });
  };

  ElementSerializer.prototype.getNamespaces = function (local) {
    var namespaces = this.namespaces,
        parent = this.parent,
        parentNamespaces;

    if (!namespaces) {
      parentNamespaces = parent && parent.getNamespaces();

      if (local || !parentNamespaces) {
        this.namespaces = namespaces = new Namespaces(parentNamespaces);
      } else {
        namespaces = parentNamespaces;
      }
    }

    return namespaces;
  };

  ElementSerializer.prototype.logNamespace = function (ns, wellknown, local) {
    var namespaces = this.getNamespaces(local);
    var nsUri = ns.uri,
        nsPrefix = ns.prefix;
    var existing = namespaces.byUri(nsUri);

    if (nsPrefix !== 'xml' && (!existing || local)) {
      namespaces.add(ns, wellknown);
    }

    namespaces.mapPrefix(nsPrefix, nsUri);
    return ns;
  };

  ElementSerializer.prototype.logNamespaceUsed = function (ns, local) {
    var element = this.element,
        model = element.$model,
        namespaces = this.getNamespaces(local); // ns may be
    //
    //   * prefix only
    //   * prefix:uri
    //   * localName only

    var prefix = ns.prefix,
        uri = ns.uri,
        newPrefix,
        idx,
        wellknownUri; // handle anonymous namespaces (elementForm=unqualified), cf. #23

    if (!prefix && !uri) {
      return {
        localName: ns.localName
      };
    }

    wellknownUri = DEFAULT_NS_MAP[prefix] || model && (model.getPackage(prefix) || {}).uri;
    uri = uri || wellknownUri || namespaces.uriByPrefix(prefix);

    if (!uri) {
      throw new Error('no namespace uri given for prefix <' + prefix + '>');
    }

    ns = namespaces.byUri(uri);

    if (!ns) {
      newPrefix = prefix;
      idx = 1; // find a prefix that is not mapped yet

      while (namespaces.uriByPrefix(newPrefix)) {
        newPrefix = prefix + '_' + idx++;
      }

      ns = this.logNamespace({
        prefix: newPrefix,
        uri: uri
      }, wellknownUri === uri);
    }

    if (prefix) {
      namespaces.mapPrefix(prefix, uri);
    }

    return ns;
  };

  ElementSerializer.prototype.parseAttributes = function (properties) {
    var self = this,
        element = this.element;
    forEach(properties, function (p) {
      var value = element.get(p.name);

      if (p.isReference) {
        if (!p.isMany) {
          value = value.id;
        } else {
          var values = [];
          forEach(value, function (v) {
            values.push(v.id);
          }); // IDREFS is a whitespace-separated list of references.

          value = values.join(' ');
        }
      }

      self.addAttribute(self.nsAttributeName(p), value);
    });
  };

  ElementSerializer.prototype.addTagName = function (nsTagName) {
    var actualNs = this.logNamespaceUsed(nsTagName);
    this.getNamespaces().logUsed(actualNs);
    return nsName(nsTagName);
  };

  ElementSerializer.prototype.addAttribute = function (name, value) {
    var attrs = this.attrs;

    if (isString(value)) {
      value = escapeAttr(value);
    }

    attrs.push({
      name: name,
      value: value
    });
  };

  ElementSerializer.prototype.serializeAttributes = function (writer) {
    var attrs = this.attrs,
        namespaces = this.namespaces;

    if (namespaces) {
      attrs = getNsAttrs(namespaces).concat(attrs);
    }

    forEach(attrs, function (a) {
      writer.append(' ').append(nsName(a.name)).append('="').append(a.value).append('"');
    });
  };

  ElementSerializer.prototype.serializeTo = function (writer) {
    var firstBody = this.body[0],
        indent = firstBody && firstBody.constructor !== BodySerializer;
    writer.appendIndent().append('<' + this.tagName);
    this.serializeAttributes(writer);
    writer.append(firstBody ? '>' : ' />');

    if (firstBody) {
      if (indent) {
        writer.appendNewLine().indent();
      }

      forEach(this.body, function (b) {
        b.serializeTo(writer);
      });

      if (indent) {
        writer.unindent().appendIndent();
      }

      writer.append('</' + this.tagName + '>');
    }

    writer.appendNewLine();
  };
  /**
   * A serializer for types that handles serialization of data types
   */


  function TypeSerializer(parent, propertyDescriptor) {
    ElementSerializer.call(this, parent, propertyDescriptor);
  }

  inherits(TypeSerializer, ElementSerializer);

  TypeSerializer.prototype.parseNsAttributes = function (element) {
    // extracted attributes
    var attributes = ElementSerializer.prototype.parseNsAttributes.call(this, element);
    var descriptor = element.$descriptor; // only serialize xsi:type if necessary

    if (descriptor.name === this.propertyDescriptor.type) {
      return attributes;
    }

    var typeNs = this.typeNs = this.nsTagName(descriptor);
    this.getNamespaces().logUsed(this.typeNs); // add xsi:type attribute to represent the elements
    // actual type

    var pkg = element.$model.getPackage(typeNs.uri),
        typePrefix = pkg.xml && pkg.xml.typePrefix || '';
    this.addAttribute(this.nsAttributeName(XSI_TYPE$1), (typeNs.prefix ? typeNs.prefix + ':' : '') + typePrefix + descriptor.ns.localName);
    return attributes;
  };

  TypeSerializer.prototype.isLocalNs = function (ns) {
    return ns.uri === (this.typeNs || this.ns).uri;
  };

  function SavingWriter() {
    this.value = '';

    this.write = function (str) {
      this.value += str;
    };
  }

  function FormatingWriter(out, format) {
    var indent = [''];

    this.append = function (str) {
      out.write(str);
      return this;
    };

    this.appendNewLine = function () {
      if (format) {
        out.write('\n');
      }

      return this;
    };

    this.appendIndent = function () {
      if (format) {
        out.write(indent.join('  '));
      }

      return this;
    };

    this.indent = function () {
      indent.push('');
      return this;
    };

    this.unindent = function () {
      indent.pop();
      return this;
    };
  }
  /**
   * A writer for meta-model backed document trees
   *
   * @param {Object} options output options to pass into the writer
   */


  function Writer(options) {
    options = assign({
      format: false,
      preamble: true
    }, options || {});

    function toXML(tree, writer) {
      var internalWriter = writer || new SavingWriter();
      var formatingWriter = new FormatingWriter(internalWriter, options.format);

      if (options.preamble) {
        formatingWriter.append(XML_PREAMBLE);
      }

      new ElementSerializer().build(tree).serializeTo(formatingWriter);

      if (!writer) {
        return internalWriter.value;
      }
    }

    return {
      toXML: toXML
    };
  }

  /**
   * A sub class of {@link Moddle} with support for import and export of DMN xml files.
   *
   * @class DmnModdle
   * @extends Moddle
   *
   * @param {Object|Array} packages to use for instantiating the model
   * @param {Object} [options] additional options to pass over
   */

  function DmnModdle(packages, options) {
    Moddle.call(this, packages, options);
  }

  DmnModdle.prototype = Object.create(Moddle.prototype);
  /**
   * Instantiates a DMN model tree from a given xml string.
   *
   * @param {String}   xmlStr
   * @param {String}   [typeName='dmn:Definitions'] name of the root element
   * @param {Object}   [options]  options to pass to the underlying reader
   * @param {Function} done       callback that is invoked with (err, result, parseContext)
   *                              once the import completes
   */

  DmnModdle.prototype.fromXML = function (xmlStr, typeName, options, done) {
    if (!isString(typeName)) {
      done = options;
      options = typeName;
      typeName = 'dmn:Definitions';
    }

    if (isFunction(options)) {
      done = options;
      options = {};
    }

    var reader = new Reader(assign({
      model: this,
      lax: true
    }, options));
    var rootHandler = reader.handler(typeName);
    reader.fromXML(xmlStr, rootHandler, done);
  };
  /**
   * Serializes a DMN object tree to XML.
   *
   * @param {String}   element    the root element, typically an instance of `Definitions`
   * @param {Object}   [options]  to pass to the underlying writer
   * @param {Function} done       callback invoked with (err, xmlStr) once the import completes
   */


  DmnModdle.prototype.toXML = function (element, options, done) {
    if (isFunction(options)) {
      done = options;
      options = {};
    }

    var writer = new Writer(options);
    var result;
    var err;

    try {
      result = writer.toXML(element);
    } catch (e) {
      err = e;
    }

    return done(err, result);
  };

  var name = "DC";
  var prefix = "dc";
  var uri = "http://www.omg.org/spec/DMN/20180521/DC/";
  var types = [{
    name: "Dimension",
    properties: [{
      name: "width",
      isAttr: true,
      type: "Real"
    }, {
      name: "height",
      isAttr: true,
      type: "Real"
    }]
  }, {
    name: "Bounds",
    properties: [{
      name: "height",
      isAttr: true,
      type: "Real"
    }, {
      name: "width",
      isAttr: true,
      type: "Real"
    }, {
      name: "x",
      isAttr: true,
      type: "Real"
    }, {
      name: "y",
      isAttr: true,
      type: "Real"
    }]
  }, {
    name: "Point",
    properties: [{
      name: "x",
      isAttr: true,
      type: "Real"
    }, {
      name: "y",
      isAttr: true,
      type: "Real"
    }]
  }, {
    name: "Color",
    properties: [{
      name: "red",
      type: "UML_Standard_Profile.mdzip:eee_1045467100323_917313_65"
    }, {
      name: "green",
      type: "UML_Standard_Profile.mdzip:eee_1045467100323_917313_65"
    }, {
      name: "blue",
      type: "UML_Standard_Profile.mdzip:eee_1045467100323_917313_65"
    }]
  }];
  var associations = [];
  var enumerations = [{
    name: "AlignmentKind",
    literalValues: [{
      name: "start"
    }, {
      name: "center"
    }, {
      name: "end"
    }]
  }];
  var DcPackage = {
    name: name,
    prefix: prefix,
    uri: uri,
    types: types,
    associations: associations,
    enumerations: enumerations
  };
  var name$1 = "DI";
  var prefix$1 = "di";
  var uri$1 = "http://www.omg.org/spec/DMN/20180521/DI/";
  var types$1 = [{
    name: "DiagramElement",
    isAbstract: true,
    properties: [{
      name: "id",
      isAttr: true,
      isId: true,
      type: "String"
    }, {
      name: "style",
      isReference: true,
      type: "Style",
      xml: {
        serialize: "property"
      }
    }, {
      name: "sharedStyle",
      isReference: true,
      isVirtual: true,
      type: "Style"
    }]
  }, {
    name: "Diagram",
    superClass: ["DiagramElement"],
    properties: [{
      name: "name",
      isAttr: true,
      type: "String"
    }, {
      name: "documentation",
      isAttr: true,
      type: "String"
    }, {
      name: "resolution",
      isAttr: true,
      type: "Real"
    }]
  }, {
    name: "Shape",
    isAbstract: true,
    properties: [{
      name: "bounds",
      type: "dc:Bounds"
    }],
    superClass: ["DiagramElement"]
  }, {
    name: "Edge",
    isAbstract: true,
    properties: [{
      name: "waypoint",
      type: "dc:Point",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }],
    superClass: ["DiagramElement"]
  }, {
    name: "Style",
    isAbstract: true,
    properties: [{
      name: "id",
      isAttr: true,
      isId: true,
      type: "String"
    }]
  }];
  var associations$1 = [];
  var enumerations$1 = [];
  var xml = {
    tagAlias: "lowerCase"
  };
  var DiPackage = {
    name: name$1,
    prefix: prefix$1,
    uri: uri$1,
    types: types$1,
    associations: associations$1,
    enumerations: enumerations$1,
    xml: xml
  };
  var name$2 = "DMN";
  var prefix$2 = "dmn";
  var uri$2 = "https://www.omg.org/spec/DMN/20191111/MODEL/";
  var types$2 = [{
    name: "AuthorityRequirement",
    superClass: ["DMNElement"],
    properties: [{
      name: "requiredAuthority",
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }, {
      name: "requiredDecision",
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }, {
      name: "requiredInput",
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "ItemDefinition",
    superClass: ["NamedElement"],
    properties: [{
      name: "typeRef",
      type: "String"
    }, {
      name: "allowedValues",
      type: "UnaryTests",
      xml: {
        serialize: "property"
      }
    }, {
      name: "typeLanguage",
      type: "String",
      isAttr: true
    }, {
      name: "itemComponent",
      type: "ItemDefinition",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "functionItem",
      type: "FunctionItem"
    }, {
      name: "isCollection",
      isAttr: true,
      type: "Boolean"
    }]
  }, {
    name: "Definitions",
    superClass: ["NamedElement"],
    properties: [{
      name: "import",
      type: "Import",
      isMany: true
    }, {
      name: "itemDefinition",
      type: "ItemDefinition",
      isMany: true
    }, {
      name: "drgElement",
      type: "DRGElement",
      isMany: true
    }, {
      name: "artifact",
      type: "Artifact",
      isMany: true
    }, {
      name: "elementCollection",
      type: "ElementCollection",
      isMany: true
    }, {
      name: "businessContextElement",
      type: "BusinessContextElement",
      isMany: true
    }, {
      name: "namespace",
      type: "String",
      isAttr: true
    }, {
      name: "expressionLanguage",
      type: "String",
      isAttr: true
    }, {
      name: "typeLanguage",
      type: "String",
      isAttr: true
    }, {
      name: "exporter",
      isAttr: true,
      type: "String"
    }, {
      name: "exporterVersion",
      isAttr: true,
      type: "String"
    }, {
      name: "dmnDI",
      type: "dmndi:DMNDI"
    }]
  }, {
    name: "KnowledgeSource",
    superClass: ["DRGElement"],
    properties: [{
      name: "authorityRequirement",
      type: "AuthorityRequirement",
      isMany: true
    }, {
      name: "type",
      type: "String"
    }, {
      name: "owner",
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }, {
      name: "locationURI",
      type: "String",
      isAttr: true
    }]
  }, {
    name: "DecisionRule",
    superClass: ["DMNElement"],
    properties: [{
      name: "inputEntry",
      type: "UnaryTests",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "outputEntry",
      type: "LiteralExpression",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "annotationEntry",
      type: "RuleAnnotation",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "Expression",
    isAbstract: true,
    superClass: ["DMNElement"],
    properties: [{
      name: "typeRef",
      isAttr: true,
      type: "String"
    }]
  }, {
    name: "InformationItem",
    superClass: ["NamedElement"],
    properties: [{
      name: "typeRef",
      isAttr: true,
      type: "String"
    }]
  }, {
    name: "Decision",
    superClass: ["DRGElement"],
    properties: [{
      name: "question",
      type: "String",
      xml: {
        serialize: "property"
      }
    }, {
      name: "allowedAnswers",
      type: "String",
      xml: {
        serialize: "property"
      }
    }, {
      name: "variable",
      type: "InformationItem",
      xml: {
        serialize: "property"
      }
    }, {
      name: "informationRequirement",
      type: "InformationRequirement",
      isMany: true
    }, {
      name: "knowledgeRequirement",
      type: "KnowledgeRequirement",
      isMany: true
    }, {
      name: "authorityRequirement",
      type: "AuthorityRequirement",
      isMany: true
    }, {
      name: "supportedObjective",
      isMany: true,
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }, {
      name: "impactedPerformanceIndicator",
      type: "DMNElementReference",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "decisionMaker",
      type: "DMNElementReference",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "decisionOwner",
      type: "DMNElementReference",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "usingProcess",
      isMany: true,
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }, {
      name: "usingTask",
      isMany: true,
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }, {
      name: "decisionLogic",
      type: "Expression"
    }]
  }, {
    name: "Invocation",
    superClass: ["Expression"],
    properties: [{
      name: "calledFunction",
      type: "Expression"
    }, {
      name: "binding",
      type: "Binding",
      isMany: true
    }]
  }, {
    name: "OrganisationalUnit",
    superClass: ["BusinessContextElement"],
    properties: [{
      name: "decisionMade",
      type: "Decision",
      isReference: true,
      isMany: true
    }, {
      name: "decisionOwned",
      type: "Decision",
      isReference: true,
      isMany: true
    }]
  }, {
    name: "Import",
    superClass: ["NamedElement"],
    properties: [{
      name: "importType",
      type: "String",
      isAttr: true
    }, {
      name: "locationURI",
      type: "String",
      isAttr: true
    }, {
      name: "namespace",
      type: "String",
      isAttr: true
    }]
  }, {
    name: "InformationRequirement",
    superClass: ["DMNElement"],
    properties: [{
      name: "requiredDecision",
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }, {
      name: "requiredInput",
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "ElementCollection",
    superClass: ["NamedElement"],
    properties: [{
      name: "drgElement",
      type: "DMNElementReference",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "DRGElement",
    isAbstract: true,
    superClass: ["NamedElement"],
    properties: []
  }, {
    name: "InputData",
    superClass: ["DRGElement"],
    properties: [{
      name: "variable",
      type: "InformationItem",
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "DMNElement",
    isAbstract: true,
    properties: [{
      name: "description",
      type: "String"
    }, {
      name: "extensionElements",
      type: "ExtensionElements"
    }, {
      name: "id",
      type: "String",
      isAttr: true,
      isId: true
    }, {
      name: "extensionAttribute",
      type: "ExtensionAttribute",
      isMany: true
    }, {
      name: "label",
      isAttr: true,
      type: "String"
    }]
  }, {
    name: "InputClause",
    superClass: ["DMNElement"],
    properties: [{
      name: "inputExpression",
      type: "LiteralExpression",
      xml: {
        serialize: "property"
      }
    }, {
      name: "inputValues",
      type: "UnaryTests",
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "DecisionTable",
    superClass: ["Expression"],
    properties: [{
      name: "input",
      type: "InputClause",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "output",
      type: "OutputClause",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "annotation",
      type: "RuleAnnotationClause",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "rule",
      type: "DecisionRule",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "hitPolicy",
      type: "HitPolicy",
      isAttr: true,
      "default": "UNIQUE"
    }, {
      name: "aggregation",
      type: "BuiltinAggregator",
      isAttr: true
    }, {
      name: "preferredOrientation",
      type: "DecisionTableOrientation",
      isAttr: true
    }, {
      name: "outputLabel",
      isAttr: true,
      type: "String"
    }]
  }, {
    name: "LiteralExpression",
    superClass: ["Expression"],
    properties: [{
      name: "expressionLanguage",
      type: "String",
      isAttr: true
    }, {
      name: "text",
      type: "String"
    }, {
      name: "importedValues",
      type: "ImportedValues"
    }]
  }, {
    name: "Binding",
    properties: [{
      name: "parameter",
      type: "InformationItem",
      xml: {
        serialize: "property"
      }
    }, {
      name: "bindingFormula",
      type: "Expression"
    }]
  }, {
    name: "KnowledgeRequirement",
    superClass: ["DMNElement"],
    properties: [{
      name: "requiredKnowledge",
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "BusinessKnowledgeModel",
    superClass: ["Invocable"],
    properties: [{
      name: "encapsulatedLogic",
      type: "FunctionDefinition",
      xml: {
        serialize: "property"
      }
    }, {
      name: "knowledgeRequirement",
      type: "KnowledgeRequirement",
      isMany: true
    }, {
      name: "authorityRequirement",
      type: "AuthorityRequirement",
      isMany: true
    }]
  }, {
    name: "BusinessContextElement",
    isAbstract: true,
    superClass: ["NamedElement"],
    properties: [{
      name: "URI",
      type: "String",
      isAttr: true
    }]
  }, {
    name: "PerformanceIndicator",
    superClass: ["BusinessContextElement"],
    properties: [{
      name: "impactingDecision",
      type: "DMNElementReference",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "FunctionDefinition",
    superClass: ["Expression"],
    properties: [{
      name: "formalParameter",
      type: "InformationItem",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "body",
      type: "Expression"
    }, {
      name: "kind",
      type: "FunctionKind",
      isAttr: true
    }]
  }, {
    name: "Context",
    superClass: ["Expression"],
    properties: [{
      name: "contextEntry",
      type: "ContextEntry",
      isMany: true
    }]
  }, {
    name: "ContextEntry",
    superClass: ["DMNElement"],
    properties: [{
      name: "variable",
      type: "InformationItem",
      xml: {
        serialize: "property"
      }
    }, {
      name: "value",
      type: "Expression"
    }]
  }, {
    name: "List",
    superClass: ["Expression"],
    properties: [{
      name: "elements",
      isMany: true,
      type: "Expression"
    }]
  }, {
    name: "Relation",
    superClass: ["Expression"],
    properties: [{
      name: "column",
      type: "InformationItem",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "row",
      type: "List",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "OutputClause",
    superClass: ["DMNElement"],
    properties: [{
      name: "outputValues",
      type: "UnaryTests",
      xml: {
        serialize: "property"
      }
    }, {
      name: "defaultOutputEntry",
      type: "LiteralExpression",
      xml: {
        serialize: "property"
      }
    }, {
      name: "name",
      isAttr: true,
      type: "String"
    }, {
      name: "typeRef",
      isAttr: true,
      type: "String"
    }]
  }, {
    name: "UnaryTests",
    superClass: ["Expression"],
    properties: [{
      name: "text",
      type: "String"
    }, {
      name: "expressionLanguage",
      type: "String",
      isAttr: true
    }]
  }, {
    name: "NamedElement",
    isAbstract: true,
    superClass: ["DMNElement"],
    properties: [{
      name: "name",
      isAttr: true,
      type: "String"
    }]
  }, {
    name: "ImportedValues",
    superClass: ["Import"],
    properties: [{
      name: "importedElement",
      type: "String"
    }, {
      name: "expressionLanguage",
      type: "String",
      isAttr: true
    }]
  }, {
    name: "DecisionService",
    superClass: ["Invocable"],
    properties: [{
      name: "outputDecision",
      type: "DMNElementReference",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "encapsulatedDecision",
      type: "DMNElementReference",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "inputDecision",
      type: "DMNElementReference",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }, {
      name: "inputData",
      type: "DMNElementReference",
      isMany: true,
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "ExtensionElements",
    properties: [{
      name: "values",
      type: "Element",
      isMany: true
    }]
  }, {
    name: "ExtensionAttribute",
    properties: [{
      name: "value",
      type: "Element"
    }, {
      name: "valueRef",
      type: "Element",
      isAttr: true,
      isReference: true
    }, {
      name: "name",
      isAttr: true,
      type: "String"
    }]
  }, {
    name: "Element",
    isAbstract: true,
    properties: [{
      name: "extensionAttribute",
      type: "ExtensionAttribute",
      isAttr: true,
      isReference: true
    }, {
      name: "elements",
      type: "ExtensionElements",
      isAttr: true,
      isReference: true
    }]
  }, {
    name: "Artifact",
    isAbstract: true,
    superClass: ["DMNElement"],
    properties: []
  }, {
    name: "Association",
    superClass: ["Artifact"],
    properties: [{
      name: "sourceRef",
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }, {
      name: "targetRef",
      type: "DMNElementReference",
      xml: {
        serialize: "property"
      }
    }, {
      name: "associationDirection",
      type: "AssociationDirection",
      isAttr: true
    }]
  }, {
    name: "TextAnnotation",
    superClass: ["Artifact"],
    properties: [{
      name: "text",
      type: "String"
    }, {
      name: "textFormat",
      isAttr: true,
      type: "String",
      "default": "text/plain"
    }]
  }, {
    name: "RuleAnnotationClause",
    properties: [{
      name: "name",
      isAttr: true,
      type: "String"
    }]
  }, {
    name: "RuleAnnotation",
    properties: [{
      name: "text",
      type: "String"
    }]
  }, {
    name: "Invocable",
    isAbstract: true,
    superClass: ["DRGElement"],
    properties: [{
      name: "variable",
      type: "InformationItem",
      xml: {
        serialize: "property"
      }
    }]
  }, {
    name: "Group",
    superClass: ["Artifact"],
    properties: [{
      name: "name",
      isAttr: true,
      type: "String"
    }]
  }, {
    name: "FunctionItem",
    superClass: ["DMNElement"],
    properties: [{
      name: "parameters",
      isMany: true,
      type: "InformationItem",
      xml: {
        serialize: "property"
      }
    }, {
      name: "outputTypeRef",
      isAttr: true,
      type: "String"
    }]
  }, {
    name: "DMNElementReference",
    properties: [{
      isAttr: true,
      name: "href",
      type: "String"
    }]
  }];
  var enumerations$2 = [{
    name: "HitPolicy",
    literalValues: [{
      name: "UNIQUE"
    }, {
      name: "FIRST"
    }, {
      name: "PRIORITY"
    }, {
      name: "ANY"
    }, {
      name: "COLLECT"
    }, {
      name: "RULE ORDER"
    }, {
      name: "OUTPUT ORDER"
    }]
  }, {
    name: "BuiltinAggregator",
    literalValues: [{
      name: "SUM"
    }, {
      name: "COUNT"
    }, {
      name: "MIN"
    }, {
      name: "MAX"
    }]
  }, {
    name: "DecisionTableOrientation",
    literalValues: [{
      name: "Rule-as-Row"
    }, {
      name: "Rule-as-Column"
    }, {
      name: "CrossTable"
    }]
  }, {
    name: "AssociationDirection",
    literalValues: [{
      name: "None"
    }, {
      name: "One"
    }, {
      name: "Both"
    }]
  }, {
    name: "FunctionKind",
    literalValues: [{
      name: "FEEL"
    }, {
      name: "Java"
    }, {
      name: "PMML"
    }]
  }];
  var associations$2 = [];
  var xml$1 = {
    tagAlias: "lowerCase"
  };
  var DmnPackage = {
    name: name$2,
    prefix: prefix$2,
    uri: uri$2,
    types: types$2,
    enumerations: enumerations$2,
    associations: associations$2,
    xml: xml$1
  };
  var name$3 = "DMNDI";
  var prefix$3 = "dmndi";
  var uri$3 = "https://www.omg.org/spec/DMN/20191111/DMNDI/";
  var types$3 = [{
    name: "DMNDI",
    properties: [{
      name: "diagrams",
      type: "DMNDiagram",
      isMany: true
    }, {
      name: "styles",
      type: "DMNStyle",
      isMany: true
    }]
  }, {
    name: "DMNStyle",
    superClass: ["di:Style"],
    properties: [{
      name: "fillColor",
      type: "dc:Color",
      isAttr: true
    }, {
      name: "strokeColor",
      type: "dc:Color",
      isAttr: true
    }, {
      name: "fontColor",
      type: "dc:Color",
      isAttr: true
    }, {
      name: "fontSize",
      isAttr: true,
      type: "Real"
    }, {
      name: "fontFamily",
      isAttr: true,
      type: "String"
    }, {
      name: "fontItalic",
      isAttr: true,
      type: "Boolean"
    }, {
      name: "fontBold",
      isAttr: true,
      type: "Boolean"
    }, {
      name: "fontUnderline",
      isAttr: true,
      type: "Boolean"
    }, {
      name: "fontStrikeThrough",
      isAttr: true,
      type: "Boolean"
    }, {
      name: "labelHorizontalAlignment",
      type: "dc:AlignmentKind",
      isAttr: true
    }, {
      name: "labelVerticalAlignment",
      type: "dc:AlignmentKind",
      isAttr: true
    }]
  }, {
    name: "DMNDiagram",
    superClass: ["di:Diagram"],
    properties: [{
      name: "dmnElementRef",
      type: "dmn:DMNElement",
      isAttr: true,
      isReference: true
    }, {
      name: "size",
      type: "Size"
    }, {
      name: "localStyle",
      type: "DMNStyle",
      isVirtual: true
    }, {
      name: "sharedStyle",
      type: "DMNStyle",
      isVirtual: true,
      isReference: true,
      redefines: "di:DiagramElement#sharedStyle"
    }, {
      name: "diagramElements",
      type: "DMNDiagramElement",
      isMany: true
    }]
  }, {
    name: "DMNDiagramElement",
    isAbstract: true,
    superClass: ["di:DiagramElement"],
    properties: [{
      name: "dmnElementRef",
      type: "dmn:DMNElement",
      isAttr: true,
      isReference: true
    }, {
      name: "sharedStyle",
      type: "DMNStyle",
      isVirtual: true,
      isReference: true,
      redefines: "di:DiagramElement#sharedStyle"
    }, {
      name: "localStyle",
      type: "DMNStyle",
      isVirtual: true
    }, {
      name: "label",
      type: "DMNLabel"
    }]
  }, {
    name: "DMNLabel",
    superClass: ["di:Shape"],
    properties: [{
      name: "text",
      type: "Text"
    }]
  }, {
    name: "DMNShape",
    superClass: ["di:Shape", "DMNDiagramElement"],
    properties: [{
      name: "isListedInputData",
      isAttr: true,
      type: "Boolean"
    }, {
      name: "decisionServiceDividerLine",
      type: "DMNDecisionServiceDividerLine"
    }, {
      name: "isCollapsed",
      isAttr: true,
      type: "Boolean"
    }]
  }, {
    name: "DMNEdge",
    superClass: ["di:Edge", "DMNDiagramElement"],
    properties: [{
      name: "sourceElement",
      type: "DMNDiagramElement",
      isAttr: true,
      isReference: true
    }, {
      name: "targetElement",
      type: "DMNDiagramElement",
      isAttr: true,
      isReference: true
    }]
  }, {
    name: "DMNDecisionServiceDividerLine",
    superClass: ["di:Edge"]
  }, {
    name: "Text",
    properties: [{
      name: "text",
      isBody: true,
      type: "String"
    }]
  }, {
    name: "Size",
    superClass: ["dc:Dimension"]
  }];
  var associations$3 = [];
  var enumerations$3 = [];
  var DmnDiPackage = {
    name: name$3,
    prefix: prefix$3,
    uri: uri$3,
    types: types$3,
    associations: associations$3,
    enumerations: enumerations$3
  };
  var name$4 = "bpmn.io DI for DMN";
  var uri$4 = "http://bpmn.io/schema/dmn/biodi/2.0";
  var prefix$4 = "biodi";
  var xml$2 = {
    tagAlias: "lowerCase"
  };
  var types$4 = [{
    name: "DecisionTable",
    isAbstract: true,
    "extends": ["dmn:DecisionTable"],
    properties: [{
      name: "annotationsWidth",
      isAttr: true,
      type: "Integer"
    }]
  }, {
    name: "OutputClause",
    isAbstract: true,
    "extends": ["dmn:OutputClause"],
    properties: [{
      name: "width",
      isAttr: true,
      type: "Integer"
    }]
  }, {
    name: "InputClause",
    isAbstract: true,
    "extends": ["dmn:InputClause"],
    properties: [{
      name: "width",
      isAttr: true,
      type: "Integer"
    }]
  }];
  var BioDiPackage = {
    name: name$4,
    uri: uri$4,
    prefix: prefix$4,
    xml: xml$2,
    types: types$4
  };
  var packages = {
    dc: DcPackage,
    di: DiPackage,
    dmn: DmnPackage,
    dmndi: DmnDiPackage,
    biodi: BioDiPackage
  };

  function simple(additionalPackages, options) {
    var pks = assign({}, packages, additionalPackages);
    return new DmnModdle(pks, options);
  }

  var name$5 = "Camunda";
  var uri$5 = "http://camunda.org/schema/1.0/dmn";
  var prefix$5 = "camunda";
  var xml$3 = {
  	tagAlias: "lowerCase"
  };
  var associations$4 = [
  ];
  var types$5 = [
  	{
  		name: "Definitions",
  		isAbstract: true,
  		"extends": [
  			"dmn:Definitions"
  		],
  		properties: [
  			{
  				name: "diagramRelationId",
  				isAttr: true,
  				type: "String"
  			}
  		]
  	},
  	{
  		name: "Decision",
  		isAbstract: true,
  		"extends": [
  			"dmn:Decision"
  		],
  		properties: [
  			{
  				name: "versionTag",
  				isAttr: true,
  				type: "String"
  			},
  			{
  				name: "historyTimeToLive",
  				isAttr: true,
  				type: "String"
  			}
  		]
  	},
  	{
  		name: "InputClause",
  		"extends": [
  			"dmn:InputClause"
  		],
  		properties: [
  			{
  				name: "inputVariable",
  				isAttr: true,
  				type: "String"
  			}
  		]
  	}
  ];
  var emumerations = [
  ];
  var CamundaModdle = {
  	name: name$5,
  	uri: uri$5,
  	prefix: prefix$5,
  	xml: xml$3,
  	associations: associations$4,
  	types: types$5,
  	emumerations: emumerations
  };

  /**
   * Set attribute `name` to `val`, or get attr `name`.
   *
   * @param {Element} el
   * @param {String} name
   * @param {String} [val]
   * @api public
   */
  function attr(el, name, val) {
    // get
    if (arguments.length == 2) {
      return el.getAttribute(name);
    } // remove


    if (val === null) {
      return el.removeAttribute(name);
    } // set


    el.setAttribute(name, val);
    return el;
  }

  var indexOf = [].indexOf;

  var indexof = function indexof(arr, obj) {
    if (indexOf) return arr.indexOf(obj);

    for (var i = 0; i < arr.length; ++i) {
      if (arr[i] === obj) return i;
    }

    return -1;
  };
  /**
   * Taken from https://github.com/component/classes
   *
   * Without the component bits.
   */

  /**
   * Whitespace regexp.
   */


  var re = /\s+/;
  /**
   * toString reference.
   */

  var toString = Object.prototype.toString;
  /**
   * Wrap `el` in a `ClassList`.
   *
   * @param {Element} el
   * @return {ClassList}
   * @api public
   */

  function classes(el) {
    return new ClassList(el);
  }
  /**
   * Initialize a new ClassList for `el`.
   *
   * @param {Element} el
   * @api private
   */


  function ClassList(el) {
    if (!el || !el.nodeType) {
      throw new Error('A DOM element reference is required');
    }

    this.el = el;
    this.list = el.classList;
  }
  /**
   * Add class `name` if not already present.
   *
   * @param {String} name
   * @return {ClassList}
   * @api public
   */


  ClassList.prototype.add = function (name) {
    // classList
    if (this.list) {
      this.list.add(name);
      return this;
    } // fallback


    var arr = this.array();
    var i = indexof(arr, name);
    if (!~i) arr.push(name);
    this.el.className = arr.join(' ');
    return this;
  };
  /**
   * Remove class `name` when present, or
   * pass a regular expression to remove
   * any which match.
   *
   * @param {String|RegExp} name
   * @return {ClassList}
   * @api public
   */


  ClassList.prototype.remove = function (name) {
    if ('[object RegExp]' == toString.call(name)) {
      return this.removeMatching(name);
    } // classList


    if (this.list) {
      this.list.remove(name);
      return this;
    } // fallback


    var arr = this.array();
    var i = indexof(arr, name);
    if (~i) arr.splice(i, 1);
    this.el.className = arr.join(' ');
    return this;
  };
  /**
   * Remove all classes matching `re`.
   *
   * @param {RegExp} re
   * @return {ClassList}
   * @api private
   */


  ClassList.prototype.removeMatching = function (re) {
    var arr = this.array();

    for (var i = 0; i < arr.length; i++) {
      if (re.test(arr[i])) {
        this.remove(arr[i]);
      }
    }

    return this;
  };
  /**
   * Toggle class `name`, can force state via `force`.
   *
   * For browsers that support classList, but do not support `force` yet,
   * the mistake will be detected and corrected.
   *
   * @param {String} name
   * @param {Boolean} force
   * @return {ClassList}
   * @api public
   */


  ClassList.prototype.toggle = function (name, force) {
    // classList
    if (this.list) {
      if ('undefined' !== typeof force) {
        if (force !== this.list.toggle(name, force)) {
          this.list.toggle(name); // toggle again to correct
        }
      } else {
        this.list.toggle(name);
      }

      return this;
    } // fallback


    if ('undefined' !== typeof force) {
      if (!force) {
        this.remove(name);
      } else {
        this.add(name);
      }
    } else {
      if (this.has(name)) {
        this.remove(name);
      } else {
        this.add(name);
      }
    }

    return this;
  };
  /**
   * Return an array of classes.
   *
   * @return {Array}
   * @api public
   */


  ClassList.prototype.array = function () {
    var className = this.el.getAttribute('class') || '';
    var str = className.replace(/^\s+|\s+$/g, '');
    var arr = str.split(re);
    if ('' === arr[0]) arr.shift();
    return arr;
  };
  /**
   * Check if class `name` is present.
   *
   * @param {String} name
   * @return {ClassList}
   * @api public
   */


  ClassList.prototype.has = ClassList.prototype.contains = function (name) {
    return this.list ? this.list.contains(name) : !!~indexof(this.array(), name);
  };
  /**
   * Remove all children from the given element.
   */


  function clear(el) {
    var c;

    while (el.childNodes.length) {
      c = el.childNodes[0];
      el.removeChild(c);
    }

    return el;
  }

  var proto = typeof Element !== 'undefined' ? Element.prototype : {};
  var vendor = proto.matches || proto.matchesSelector || proto.webkitMatchesSelector || proto.mozMatchesSelector || proto.msMatchesSelector || proto.oMatchesSelector;
  var matchesSelector = match;
  /**
   * Match `el` to `selector`.
   *
   * @param {Element} el
   * @param {String} selector
   * @return {Boolean}
   * @api public
   */

  function match(el, selector) {
    if (!el || el.nodeType !== 1) return false;
    if (vendor) return vendor.call(el, selector);
    var nodes = el.parentNode.querySelectorAll(selector);

    for (var i = 0; i < nodes.length; i++) {
      if (nodes[i] == el) return true;
    }

    return false;
  }
  /**
   * Closest
   *
   * @param {Element} el
   * @param {String} selector
   * @param {Boolean} checkYourSelf (optional)
   */


  function closest(element, selector, checkYourSelf) {
    var currentElem = checkYourSelf ? element : element.parentNode;

    while (currentElem && currentElem.nodeType !== document.DOCUMENT_NODE && currentElem.nodeType !== document.DOCUMENT_FRAGMENT_NODE) {
      if (matchesSelector(currentElem, selector)) {
        return currentElem;
      }

      currentElem = currentElem.parentNode;
    }

    return matchesSelector(currentElem, selector) ? currentElem : null;
  }

  var bind$1 = window.addEventListener ? 'addEventListener' : 'attachEvent',
      unbind = window.removeEventListener ? 'removeEventListener' : 'detachEvent',
      prefix$6 = bind$1 !== 'addEventListener' ? 'on' : '';
  /**
   * Bind `el` event `type` to `fn`.
   *
   * @param {Element} el
   * @param {String} type
   * @param {Function} fn
   * @param {Boolean} capture
   * @return {Function}
   * @api public
   */

  var bind_1 = function bind_1(el, type, fn, capture) {
    el[bind$1](prefix$6 + type, fn, capture || false);
    return fn;
  };
  /**
   * Unbind `el` event `type`'s callback `fn`.
   *
   * @param {Element} el
   * @param {String} type
   * @param {Function} fn
   * @param {Boolean} capture
   * @return {Function}
   * @api public
   */


  var unbind_1 = function unbind_1(el, type, fn, capture) {
    el[unbind](prefix$6 + type, fn, capture || false);
    return fn;
  };

  var componentEvent = {
    bind: bind_1,
    unbind: unbind_1
  };
  /**
   * Module dependencies.
   */

  /**
   * Delegate event `type` to `selector`
   * and invoke `fn(e)`. A callback function
   * is returned which may be passed to `.unbind()`.
   *
   * @param {Element} el
   * @param {String} selector
   * @param {String} type
   * @param {Function} fn
   * @param {Boolean} capture
   * @return {Function}
   * @api public
   */
  // Some events don't bubble, so we want to bind to the capture phase instead
  // when delegating.

  var forceCaptureEvents = ['focus', 'blur'];

  function bind$1$1(el, selector, type, fn, capture) {
    if (forceCaptureEvents.indexOf(type) !== -1) {
      capture = true;
    }

    return componentEvent.bind(el, type, function (e) {
      var target = e.target || e.srcElement;
      e.delegateTarget = closest(target, selector, true);

      if (e.delegateTarget) {
        fn.call(el, e);
      }
    }, capture);
  }
  /**
   * Unbind event `type`'s callback `fn`.
   *
   * @param {Element} el
   * @param {String} type
   * @param {Function} fn
   * @param {Boolean} capture
   * @api public
   */


  function unbind$1(el, type, fn, capture) {
    if (forceCaptureEvents.indexOf(type) !== -1) {
      capture = true;
    }

    return componentEvent.unbind(el, type, fn, capture);
  }

  var delegate = {
    bind: bind$1$1,
    unbind: unbind$1
  };
  /**
   * Expose `parse`.
   */

  var domify = parse;
  /**
   * Tests for browser support.
   */

  var innerHTMLBug = false;
  var bugTestDiv;

  if (typeof document !== 'undefined') {
    bugTestDiv = document.createElement('div'); // Setup

    bugTestDiv.innerHTML = '  <link/><table></table><a href="/a">a</a><input type="checkbox"/>'; // Make sure that link elements get serialized correctly by innerHTML
    // This requires a wrapper element in IE

    innerHTMLBug = !bugTestDiv.getElementsByTagName('link').length;
    bugTestDiv = undefined;
  }
  /**
   * Wrap map from jquery.
   */


  var map$1 = {
    legend: [1, '<fieldset>', '</fieldset>'],
    tr: [2, '<table><tbody>', '</tbody></table>'],
    col: [2, '<table><tbody></tbody><colgroup>', '</colgroup></table>'],
    // for script/link/style tags to work in IE6-8, you have to wrap
    // in a div with a non-whitespace character in front, ha!
    _default: innerHTMLBug ? [1, 'X<div>', '</div>'] : [0, '', '']
  };
  map$1.td = map$1.th = [3, '<table><tbody><tr>', '</tr></tbody></table>'];
  map$1.option = map$1.optgroup = [1, '<select multiple="multiple">', '</select>'];
  map$1.thead = map$1.tbody = map$1.colgroup = map$1.caption = map$1.tfoot = [1, '<table>', '</table>'];
  map$1.polyline = map$1.ellipse = map$1.polygon = map$1.circle = map$1.text = map$1.line = map$1.path = map$1.rect = map$1.g = [1, '<svg xmlns="http://www.w3.org/2000/svg" version="1.1">', '</svg>'];
  /**
   * Parse `html` and return a DOM Node instance, which could be a TextNode,
   * HTML DOM Node of some kind (<div> for example), or a DocumentFragment
   * instance, depending on the contents of the `html` string.
   *
   * @param {String} html - HTML string to "domify"
   * @param {Document} doc - The `document` instance to create the Node for
   * @return {DOMNode} the TextNode, DOM Node, or DocumentFragment instance
   * @api private
   */

  function parse(html, doc) {
    if ('string' != typeof html) throw new TypeError('String expected'); // default to the global `document` object

    if (!doc) doc = document; // tag name

    var m = /<([\w:]+)/.exec(html);
    if (!m) return doc.createTextNode(html);
    html = html.replace(/^\s+|\s+$/g, ''); // Remove leading/trailing whitespace

    var tag = m[1]; // body support

    if (tag == 'body') {
      var el = doc.createElement('html');
      el.innerHTML = html;
      return el.removeChild(el.lastChild);
    } // wrap map


    var wrap = map$1[tag] || map$1._default;
    var depth = wrap[0];
    var prefix = wrap[1];
    var suffix = wrap[2];
    var el = doc.createElement('div');
    el.innerHTML = prefix + html + suffix;

    while (depth--) {
      el = el.lastChild;
    } // one element


    if (el.firstChild == el.lastChild) {
      return el.removeChild(el.firstChild);
    } // several elements


    var fragment = doc.createDocumentFragment();

    while (el.firstChild) {
      fragment.appendChild(el.removeChild(el.firstChild));
    }

    return fragment;
  }

  function query(selector, el) {
    el = el || document;
    return el.querySelector(selector);
  }

  function all(selector, el) {
    el = el || document;
    return el.querySelectorAll(selector);
  }

  function remove(el) {
    el.parentNode && el.parentNode.removeChild(el);
  }

  function ownKeys(object, enumerableOnly) {
    var keys = Object.keys(object);

    if (Object.getOwnPropertySymbols) {
      var symbols = Object.getOwnPropertySymbols(object);
      if (enumerableOnly) symbols = symbols.filter(function (sym) {
        return Object.getOwnPropertyDescriptor(object, sym).enumerable;
      });
      keys.push.apply(keys, symbols);
    }

    return keys;
  }

  function _objectSpread(target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i] != null ? arguments[i] : {};

      if (i % 2) {
        ownKeys(source, true).forEach(function (key) {
          _defineProperty(target, key, source[key]);
        });
      } else if (Object.getOwnPropertyDescriptors) {
        Object.defineProperties(target, Object.getOwnPropertyDescriptors(source));
      } else {
        ownKeys(source).forEach(function (key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
    }

    return target;
  }

  function _toConsumableArray(arr) {
    return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _nonIterableSpread();
  }

  function _nonIterableSpread() {
    throw new TypeError("Invalid attempt to spread non-iterable instance");
  }

  function _iterableToArray(iter) {
    if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter);
  }

  function _arrayWithoutHoles(arr) {
    if (Array.isArray(arr)) {
      for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) {
        arr2[i] = arr[i];
      }

      return arr2;
    }
  }

  function _typeof$1(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$1 = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$1 = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$1(obj);
  }

  function _classCallCheck$1(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$1(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$1(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$1(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$1(Constructor, staticProps);
    return Constructor;
  }

  function _defineProperty(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }
  var DEFAULT_CONTAINER_OPTIONS = {
    width: '100%',
    height: '100%',
    position: 'relative'
  };
  /**
   * The base class for DMN viewers and editors.
   *
   * @abstract
   */

  var Manager =
  /*#__PURE__*/
  function () {
    /**
     * Create a new instance with the given options.
     *
     * @param  {Object} options
     *
     * @return {Manager}
     */
    function Manager() {
      var _this = this;

      var options = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

      _classCallCheck$1(this, Manager);

      _defineProperty(this, "_viewsChanged", function () {
        _this._emit('views.changed', {
          views: _this._views,
          activeView: _this._activeView
        });
      });

      this._eventBus = new EventBus();
      this._viewsChanged = debounce(this._viewsChanged, 0);
      this._views = [];
      this._viewers = {};

      this._init(options);
    }
    /**
     * Parse and render a DMN diagram.
     *
     * Once finished the viewer reports back the result to the
     * provided callback function with (err, warnings).
     *
     * ## Life-Cycle Events
     *
     * During import the viewer will fire life-cycle events:
     *
     *   * import.parse.start (about to read model from xml)
     *   * import.parse.complete (model read; may have worked or not)
     *   * import.render.start (graphical import start)
     *   * import.render.complete (graphical import finished)
     *   * import.done (everything done)
     *
     * You can use these events to hook into the life-cycle.
     *
     * @param {string} xml the DMN xml
     * @param {Object} [options]
     * @param {boolean} [options.open=true]
     * @param {Function} [done] invoked with (err, warnings=[])
     */


    _createClass$1(Manager, [{
      key: "importXML",
      value: function importXML(xml, options, done) {
        var _this2 = this;

        if (_typeof$1(options) !== 'object') {
          done = options;
          options = {
            open: true
          };
        }

        if (typeof done !== 'function') {
          done = noop;
        } // hook in pre-parse listeners +
        // allow xml manipulation


        xml = this._emit('import.parse.start', {
          xml: xml
        }) || xml;

        this._moddle.fromXML(xml, 'dmn:Definitions', function (err, definitions, context) {
          // hook in post parse listeners +
          // allow definitions manipulation
          definitions = _this2._emit('import.parse.complete', {
            error: err,
            definitions: definitions,
            context: context
          }) || definitions;
          var parseWarnings = context.warnings;

          _this2._setDefinitions(definitions);

          if (err) {
            err = checkDMNCompatibilityError(err, xml) || checkValidationError(err) || err;
          }

          if (err || !options.open) {
            _this2._emit('import.done', {
              error: err,
              warmings: parseWarnings
            });

            return done(err, parseWarnings);
          }

          var view = _this2._activeView || _this2._getInitialView(_this2._views);

          if (!view) {
            return done(new Error('no displayable contents'));
          }

          _this2.open(view, function (err, warnings) {
            var allWarnings = [].concat(parseWarnings, warnings || []);

            _this2._emit('import.done', {
              error: err,
              warnings: allWarnings
            });

            done(err, allWarnings);
          });
        });
      }
    }, {
      key: "getDefinitions",
      value: function getDefinitions() {
        return this._definitions;
      }
      /**
       * Return active view.
       *
       * @return {View}
       */

    }, {
      key: "getActiveView",
      value: function getActiveView() {
        return this._activeView;
      }
      /**
       * Get the currently active viewer instance.
       *
       * @return {View}
       */

    }, {
      key: "getActiveViewer",
      value: function getActiveViewer() {
        var activeView = this.getActiveView();
        return activeView && this._getViewer(activeView);
      }
    }, {
      key: "getView",
      value: function getView(element) {
        return this._views.filter(function (v) {
          return v.element === element;
        })[0];
      }
    }, {
      key: "getViews",
      value: function getViews() {
        return this._views;
      }
      /**
       * Export the currently displayed DMN diagram as
       * a DMN XML document.
       *
       * ## Life-Cycle Events
       *
       * During XML saving the viewer will fire life-cycle events:
       *
       *   * saveXML.start (before serialization)
       *   * saveXML.serialized (after xml generation)
       *   * saveXML.done (everything done)
       *
       * You can use these events to hook into the life-cycle.
       *
       * @param {Object} [options] export options
       * @param {boolean} [options.format=false] output formated XML
       * @param {boolean} [options.preamble=true] output preamble
       * @param {Function} done invoked with (err, xml)
       */

    }, {
      key: "saveXML",
      value: function saveXML(options, done) {
        var _this3 = this;

        if (typeof options === 'function') {
          done = options;
          options = {};
        }

        var definitions = this._definitions;

        if (!definitions) {
          return done(new Error('no definitions loaded'));
        } // allow to fiddle around with definitions


        definitions = this._emit('saveXML.start', {
          definitions: definitions
        }) || definitions;

        this._moddle.toXML(definitions, options, function (err, xml) {
          try {
            xml = _this3._emit('saveXML.serialized', {
              error: err,
              xml: xml
            }) || xml;

            _this3._emit('saveXML.done', {
              error: err,
              xml: xml
            });
          } catch (e) {
            console.error('error in saveXML life-cycle listener', e);
          }

          done(err, xml);
        });
      }
      /**
       * Register an event listener
       *
       * Remove a previously added listener via {@link #off(event, callback)}.
       *
       * @param {string} event
       * @param {number} [priority]
       * @param {Function} callback
       * @param {Object} [that]
       */

    }, {
      key: "on",
      value: function on() {
        var _this$_eventBus;

        (_this$_eventBus = this._eventBus).on.apply(_this$_eventBus, arguments);
      }
      /**
       * De-register an event listener
       *
       * @param {string} event
       * @param {Function} callback
       */

    }, {
      key: "off",
      value: function off() {
        var _this$_eventBus2;

        (_this$_eventBus2 = this._eventBus).off.apply(_this$_eventBus2, arguments);
      }
      /**
       * Register a listener to be invoked once only.
       *
       * @param {string} event
       * @param {number} [priority]
       * @param {Function} callback
       * @param {Object} [that]
       */

    }, {
      key: "once",
      value: function once() {
        var _this$_eventBus3;

        (_this$_eventBus3 = this._eventBus).once.apply(_this$_eventBus3, arguments);
      }
    }, {
      key: "attachTo",
      value: function attachTo(parentNode) {
        // unwrap jQuery if provided
        if (parentNode.get && parentNode.constructor.prototype.jquery) {
          parentNode = parentNode.get(0);
        }

        if (typeof parentNode === 'string') {
          parentNode = query(parentNode);
        }

        parentNode.appendChild(this._container);

        this._emit('attach', {});
      }
    }, {
      key: "detach",
      value: function detach() {
        this._emit('detach', {});

        remove(this._container);
      }
    }, {
      key: "destroy",
      value: function destroy() {
        var _this4 = this;

        Object.keys(this._viewers).forEach(function (viewerId) {
          var viewer = _this4._viewers[viewerId];
          safeExecute(viewer, 'destroy');
        });
        remove(this._container);
      }
    }, {
      key: "_init",
      value: function _init(options) {
        this._options = options;
        this._moddle = this._createModdle(options);
        this._viewers = {};
        this._views = [];
        var container = domify('<div class="dmn-js-parent"></div>');
        var containerOptions = assign({}, DEFAULT_CONTAINER_OPTIONS, options);
        assign(container.style, {
          width: ensureUnit(containerOptions.width),
          height: ensureUnit(containerOptions.height),
          position: containerOptions.position
        });
        this._container = container;

        if (options.container) {
          this.attachTo(options.container);
        }
      }
      /**
       * Open diagram element.
       *
       * @param  {ModdleElement}   element
       * @param  {Function} [done]
       */

    }, {
      key: "open",
      value: function open(view) {
        var done = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : noop;

        this._switchView(view, done);
      }
    }, {
      key: "_setDefinitions",
      value: function _setDefinitions(definitions) {
        this._definitions = definitions;

        this._updateViews();
      }
    }, {
      key: "_updateViews",

      /**
       * Recompute changed views after elements in
       * the DMN diagram have changed.
       */
      value: function _updateViews() {
        var definitions = this._definitions;

        if (!definitions) {
          this._views = [];

          this._switchView(null);

          return;
        }

        var viewProviders = this._getViewProviders();

        var displayableElements = [definitions].concat(_toConsumableArray(definitions.drgElement || [])); // compute list of available views

        var views = this._views,
            newViews = [];
        var _iteratorNormalCompletion = true;
        var _didIteratorError = false;
        var _iteratorError = undefined;

        try {
          for (var _iterator = displayableElements[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
            var element = _step.value;
            var provider = find(viewProviders, function (provider) {
              if (typeof provider.opens === 'string') {
                return provider.opens === element.$type;
              } else {
                return provider.opens(element);
              }
            });

            if (!provider) {
              continue;
            }

            var view = {
              element: element,
              id: element.id,
              name: element.name,
              type: provider.id
            };
            newViews.push(view);
          }
        } catch (err) {
          _didIteratorError = true;
          _iteratorError = err;
        } finally {
          try {
            if (!_iteratorNormalCompletion && _iterator["return"] != null) {
              _iterator["return"]();
            }
          } finally {
            if (_didIteratorError) {
              throw _iteratorError;
            }
          }
        }

        var activeView = this._activeView,
            newActiveView;

        if (activeView) {
          // check the new active view
          newActiveView = find(newViews, function (view) {
            return viewsEqual(activeView, view);
          }) || this._getInitialView(newViews);

          if (!newActiveView) {
            return this._switchView(null);
          }
        } // Views have changed if
        // active view has changed OR
        // number of views has changed OR
        // not all views equal


        var activeViewChanged = !viewsEqual(activeView, newActiveView) || viewNameChanged(activeView, newActiveView);
        var viewsChanged = views.length !== newViews.length || !every(newViews, function (newView) {
          return find(views, function (view) {
            return viewsEqual(view, newView) && !viewNameChanged(view, newView);
          });
        });
        this._activeView = newActiveView;
        this._views = newViews;

        if (activeViewChanged || viewsChanged) {
          this._viewsChanged();
        }
      }
    }, {
      key: "_getInitialView",
      value: function _getInitialView(views) {
        return views[0];
      }
      /**
       * Switch to another view.
       *
       * @param  {View} newView
       * @param  {Function} [done]
       */

    }, {
      key: "_switchView",
      value: function _switchView(newView) {
        var _this5 = this;

        var done = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : noop;

        var complete = function complete(err, warnings) {
          _this5._viewsChanged();

          done(err, warnings);
        };

        var activeView = this.getActiveView(),
            activeViewer;

        var newViewer = newView && this._getViewer(newView),
            element = newView && newView.element;

        if (activeView) {
          activeViewer = this._getViewer(activeView);

          if (activeViewer !== newViewer) {
            safeExecute(activeViewer, 'clear');
            activeViewer.detach();
          }
        }

        this._activeView = newView;

        if (newViewer) {
          if (activeViewer !== newViewer) {
            newViewer.attachTo(this._container);
          }

          this._emit('import.render.start', {
            view: newView,
            element: element
          });

          return newViewer.open(element, function (err, warnings) {
            _this5._emit('import.render.complete', {
              view: newView,
              error: err,
              warnings: warnings
            });

            complete(err, warnings);
          });
        } // no active view


        complete();
      }
    }, {
      key: "_getViewer",
      value: function _getViewer(view) {
        var type = view.type;
        var viewer = this._viewers[type];

        if (!viewer) {
          viewer = this._viewers[type] = this._createViewer(view.type);

          this._emit('viewer.created', {
            type: type,
            viewer: viewer
          });
        }

        return viewer;
      }
    }, {
      key: "_createViewer",
      value: function _createViewer(id) {
        var provider = find(this._getViewProviders(), function (provider) {
          return provider.id === id;
        });

        if (!provider) {
          throw new Error('no provider for view type <' + id + '>');
        }

        var Viewer = provider.constructor;
        var providerOptions = this._options[id] || {};
        var commonOptions = this._options.common || {};
        return new Viewer(_objectSpread({}, commonOptions, {}, providerOptions, {
          additionalModules: [].concat(_toConsumableArray(providerOptions.additionalModules || []), [{
            _parent: ['value', this],
            moddle: ['value', this._moddle]
          }])
        }));
      }
      /**
       * Emit an event.
       */

    }, {
      key: "_emit",
      value: function _emit() {
        var _this$_eventBus4;

        return (_this$_eventBus4 = this._eventBus).fire.apply(_this$_eventBus4, arguments);
      }
    }, {
      key: "_createModdle",
      value: function _createModdle(options) {
        return new simple(assign({
          camunda: CamundaModdle
        }, options.moddleExtensions));
      }
      /**
       * Return the list of available view providers.
       *
       * @abstract
       *
       * @return {Array<ViewProvider>}
       */

    }, {
      key: "_getViewProviders",
      value: function _getViewProviders() {
        return [];
      }
    }]);

    return Manager;
  }(); // helpers //////////////////////

  function noop() {}
  /**
   * Ensure the passed argument is a proper unit (defaulting to px)
   */


  function ensureUnit(val) {
    return val + (isNumber(val) ? 'px' : '');
  }

  function checkDMNCompatibilityError(err, xml) {
    // check if we can indicate opening of old DMN 1.1 or DMN 1.2 diagrams
    if (err.message !== 'failed to parse document as <dmn:Definitions>') {
      return null;
    }

    var olderDMNVersion = xml.indexOf('"http://www.omg.org/spec/DMN/20151101/dmn.xsd"') !== -1 && '1.1' || xml.indexOf('"http://www.omg.org/spec/DMN/20180521/MODEL/"') !== -1 && '1.2';

    if (!olderDMNVersion) {
      return null;
    }

    err = new Error('unsupported DMN ' + olderDMNVersion + ' file detected; ' + 'only DMN 1.3 files can be opened');
    console.error('Cannot open what looks like a DMN ' + olderDMNVersion + ' diagram. ' + 'Please refer to https://bpmn.io/l/dmn-compatibility.html ' + 'to learn how to make the toolkit compatible with older DMN files', err);
    return err;
  }

  function checkValidationError(err) {
    // check if we can help the user by indicating wrong DMN 1.3 xml
    // (in case he or the exporting tool did not get that right)
    var pattern = /unparsable content <([^>]+)> detected([\s\S]*)$/,
        match = pattern.exec(err.message);

    if (!match) {
      return null;
    }

    err.message = 'unparsable content <' + match[1] + '> detected; ' + 'this may indicate an invalid DMN 1.3 diagram file' + match[2];
    return err;
  }

  function viewsEqual(a, b) {
    if (!isDefined(a)) {
      if (!isDefined(b)) {
        return true;
      } else {
        return false;
      }
    }

    if (!isDefined(b)) {
      return false;
    } // compare by element OR element ID equality


    return a.element === b.element || a.id === b.id;
  }

  function viewNameChanged(a, b) {
    return !a || !b || a.name !== b.name;
  }

  function safeExecute(viewer, method) {
    if (isFunction(viewer[method])) {
      viewer[method]();
    }
  }

  var CLASS_PATTERN = /^class /;

  function isClass(fn) {
    return CLASS_PATTERN.test(fn.toString());
  }

  function isArray$1(obj) {
    return Object.prototype.toString.call(obj) === '[object Array]';
  }

  function hasOwnProp(obj, prop) {
    return Object.prototype.hasOwnProperty.call(obj, prop);
  }

  function annotate() {
    var args = Array.prototype.slice.call(arguments);

    if (args.length === 1 && isArray$1(args[0])) {
      args = args[0];
    }

    var fn = args.pop();
    fn.$inject = args;
    return fn;
  } // Current limitations:
  // - can't put into "function arg" comments
  // function /* (no parenthesis like this) */ (){}
  // function abc( /* xx (no parenthesis like this) */ a, b) {}
  //
  // Just put the comment before function or inside:
  // /* (((this is fine))) */ function(a, b) {}
  // function abc(a) { /* (((this is fine))) */}
  //
  // - can't reliably auto-annotate constructor; we'll match the
  // first constructor(...) pattern found which may be the one
  // of a nested class, too.


  var CONSTRUCTOR_ARGS = /constructor\s*[^(]*\(\s*([^)]*)\)/m;
  var FN_ARGS = /^(?:async )?(?:function\s*)?[^(]*\(\s*([^)]*)\)/m;
  var FN_ARG = /\/\*([^*]*)\*\//m;

  function parseAnnotations(fn) {
    if (typeof fn !== 'function') {
      throw new Error('Cannot annotate "' + fn + '". Expected a function!');
    }

    var match = fn.toString().match(isClass(fn) ? CONSTRUCTOR_ARGS : FN_ARGS); // may parse class without constructor

    if (!match) {
      return [];
    }

    return match[1] && match[1].split(',').map(function (arg) {
      match = arg.match(FN_ARG);
      return match ? match[1].trim() : arg.trim();
    }) || [];
  }

  function Module() {
    var providers = [];

    this.factory = function (name, factory) {
      providers.push([name, 'factory', factory]);
      return this;
    };

    this.value = function (name, value) {
      providers.push([name, 'value', value]);
      return this;
    };

    this.type = function (name, type) {
      providers.push([name, 'type', type]);
      return this;
    };

    this.forEach = function (iterator) {
      providers.forEach(iterator);
    };
  }

  function Injector(modules, parent) {
    parent = parent || {
      get: function get(name, strict) {
        currentlyResolving.push(name);

        if (strict === false) {
          return null;
        } else {
          throw error('No provider for "' + name + '"!');
        }
      }
    };
    var currentlyResolving = [];
    var providers = this._providers = Object.create(parent._providers || null);
    var instances = this._instances = Object.create(null);
    var self = instances.injector = this;

    var error = function error(msg) {
      var stack = currentlyResolving.join(' -> ');
      currentlyResolving.length = 0;
      return new Error(stack ? msg + ' (Resolving: ' + stack + ')' : msg);
    };
    /**
     * Return a named service.
     *
     * @param {String} name
     * @param {Boolean} [strict=true] if false, resolve missing services to null
     *
     * @return {Object}
     */


    var get = function get(name, strict) {
      if (!providers[name] && name.indexOf('.') !== -1) {
        var parts = name.split('.');
        var pivot = get(parts.shift());

        while (parts.length) {
          pivot = pivot[parts.shift()];
        }

        return pivot;
      }

      if (hasOwnProp(instances, name)) {
        return instances[name];
      }

      if (hasOwnProp(providers, name)) {
        if (currentlyResolving.indexOf(name) !== -1) {
          currentlyResolving.push(name);
          throw error('Cannot resolve circular dependency!');
        }

        currentlyResolving.push(name);
        instances[name] = providers[name][0](providers[name][1]);
        currentlyResolving.pop();
        return instances[name];
      }

      return parent.get(name, strict);
    };

    var fnDef = function fnDef(fn, locals) {
      if (typeof locals === 'undefined') {
        locals = {};
      }

      if (typeof fn !== 'function') {
        if (isArray$1(fn)) {
          fn = annotate(fn.slice());
        } else {
          throw new Error('Cannot invoke "' + fn + '". Expected a function!');
        }
      }

      var inject = fn.$inject || parseAnnotations(fn);
      var dependencies = inject.map(function (dep) {
        if (hasOwnProp(locals, dep)) {
          return locals[dep];
        } else {
          return get(dep);
        }
      });
      return {
        fn: fn,
        dependencies: dependencies
      };
    };

    var instantiate = function instantiate(Type) {
      var def = fnDef(Type);
      var fn = def.fn,
          dependencies = def.dependencies; // instantiate var args constructor

      var Constructor = Function.prototype.bind.apply(fn, [null].concat(dependencies));
      return new Constructor();
    };

    var invoke = function invoke(func, context, locals) {
      var def = fnDef(func, locals);
      var fn = def.fn,
          dependencies = def.dependencies;
      return fn.apply(context, dependencies);
    };

    var createPrivateInjectorFactory = function createPrivateInjectorFactory(privateChildInjector) {
      return annotate(function (key) {
        return privateChildInjector.get(key);
      });
    };

    var createChild = function createChild(modules, forceNewInstances) {
      if (forceNewInstances && forceNewInstances.length) {
        var fromParentModule = Object.create(null);
        var matchedScopes = Object.create(null);
        var privateInjectorsCache = [];
        var privateChildInjectors = [];
        var privateChildFactories = [];
        var provider;
        var cacheIdx;
        var privateChildInjector;
        var privateChildInjectorFactory;

        for (var name in providers) {
          provider = providers[name];

          if (forceNewInstances.indexOf(name) !== -1) {
            if (provider[2] === 'private') {
              cacheIdx = privateInjectorsCache.indexOf(provider[3]);

              if (cacheIdx === -1) {
                privateChildInjector = provider[3].createChild([], forceNewInstances);
                privateChildInjectorFactory = createPrivateInjectorFactory(privateChildInjector);
                privateInjectorsCache.push(provider[3]);
                privateChildInjectors.push(privateChildInjector);
                privateChildFactories.push(privateChildInjectorFactory);
                fromParentModule[name] = [privateChildInjectorFactory, name, 'private', privateChildInjector];
              } else {
                fromParentModule[name] = [privateChildFactories[cacheIdx], name, 'private', privateChildInjectors[cacheIdx]];
              }
            } else {
              fromParentModule[name] = [provider[2], provider[1]];
            }

            matchedScopes[name] = true;
          }

          if ((provider[2] === 'factory' || provider[2] === 'type') && provider[1].$scope) {
            /* jshint -W083 */
            forceNewInstances.forEach(function (scope) {
              if (provider[1].$scope.indexOf(scope) !== -1) {
                fromParentModule[name] = [provider[2], provider[1]];
                matchedScopes[scope] = true;
              }
            });
          }
        }

        forceNewInstances.forEach(function (scope) {
          if (!matchedScopes[scope]) {
            throw new Error('No provider for "' + scope + '". Cannot use provider from the parent!');
          }
        });
        modules.unshift(fromParentModule);
      }

      return new Injector(modules, self);
    };

    var factoryMap = {
      factory: invoke,
      type: instantiate,
      value: function value(_value) {
        return _value;
      }
    };
    modules.forEach(function (module) {
      function arrayUnwrap(type, value) {
        if (type !== 'value' && isArray$1(value)) {
          value = annotate(value.slice());
        }

        return value;
      } // TODO(vojta): handle wrong inputs (modules)


      if (module instanceof Module) {
        module.forEach(function (provider) {
          var name = provider[0];
          var type = provider[1];
          var value = provider[2];
          providers[name] = [factoryMap[type], arrayUnwrap(type, value), type];
        });
      } else if (_typeof(module) === 'object') {
        if (module.__exports__) {
          var clonedModule = Object.keys(module).reduce(function (m, key) {
            if (key.substring(0, 2) !== '__') {
              m[key] = module[key];
            }

            return m;
          }, Object.create(null));
          var privateInjector = new Injector((module.__modules__ || []).concat([clonedModule]), self);
          var getFromPrivateInjector = annotate(function (key) {
            return privateInjector.get(key);
          });

          module.__exports__.forEach(function (key) {
            providers[key] = [getFromPrivateInjector, key, 'private', privateInjector];
          });
        } else {
          Object.keys(module).forEach(function (name) {
            if (module[name][2] === 'private') {
              providers[name] = module[name];
              return;
            }

            var type = module[name][0];
            var value = module[name][1];
            providers[name] = [factoryMap[type], arrayUnwrap(type, value), type];
          });
        }
      }
    }); // public API

    this.get = get;
    this.invoke = invoke;
    this.instantiate = instantiate;
    this.createChild = createChild;
  }

  function createCommonjsModule(fn, module) {
  	return module = { exports: {} }, fn(module, module.exports), module.exports;
  }

  var inherits_browser = createCommonjsModule(function (module) {
    if (typeof Object.create === 'function') {
      // implementation from standard node.js 'util' module
      module.exports = function inherits(ctor, superCtor) {
        if (superCtor) {
          ctor.super_ = superCtor;
          ctor.prototype = Object.create(superCtor.prototype, {
            constructor: {
              value: ctor,
              enumerable: false,
              writable: true,
              configurable: true
            }
          });
        }
      };
    } else {
      // old school shim for old browsers
      module.exports = function inherits(ctor, superCtor) {
        if (superCtor) {
          ctor.super_ = superCtor;

          var TempCtor = function TempCtor() {};

          TempCtor.prototype = superCtor.prototype;
          ctor.prototype = new TempCtor();
          ctor.prototype.constructor = ctor;
        }
      };
    }
  });

  var DEFAULT_RENDER_PRIORITY = 1000;
  /**
   * The base implementation of shape and connection renderers.
   *
   * @param {EventBus} eventBus
   * @param {number} [renderPriority=1000]
   */

  function BaseRenderer(eventBus, renderPriority) {
    var self = this;
    renderPriority = renderPriority || DEFAULT_RENDER_PRIORITY;
    eventBus.on(['render.shape', 'render.connection'], renderPriority, function (evt, context) {
      var type = evt.type,
          element = context.element,
          visuals = context.gfx;

      if (self.canRender(element)) {
        if (type === 'render.shape') {
          return self.drawShape(visuals, element);
        } else {
          return self.drawConnection(visuals, element);
        }
      }
    });
    eventBus.on(['render.getShapePath', 'render.getConnectionPath'], renderPriority, function (evt, element) {
      if (self.canRender(element)) {
        if (evt.type === 'render.getShapePath') {
          return self.getShapePath(element);
        } else {
          return self.getConnectionPath(element);
        }
      }
    });
  }
  /**
   * Should check whether *this* renderer can render
   * the element/connection.
   *
   * @param {element} element
   *
   * @returns {boolean}
   */

  BaseRenderer.prototype.canRender = function () {};
  /**
   * Provides the shape's snap svg element to be drawn on the `canvas`.
   *
   * @param {djs.Graphics} visuals
   * @param {Shape} shape
   *
   * @returns {Snap.svg} [returns a Snap.svg paper element ]
   */


  BaseRenderer.prototype.drawShape = function () {};
  /**
   * Provides the shape's snap svg element to be drawn on the `canvas`.
   *
   * @param {djs.Graphics} visuals
   * @param {Connection} connection
   *
   * @returns {Snap.svg} [returns a Snap.svg paper element ]
   */


  BaseRenderer.prototype.drawConnection = function () {};
  /**
   * Gets the SVG path of a shape that represents it's visual bounds.
   *
   * @param {Shape} shape
   *
   * @return {string} svg path
   */


  BaseRenderer.prototype.getShapePath = function () {};
  /**
   * Gets the SVG path of a connection that represents it's visual bounds.
   *
   * @param {Connection} connection
   *
   * @return {string} svg path
   */


  BaseRenderer.prototype.getConnectionPath = function () {};

  function ensureImported(element, target) {
    if (element.ownerDocument !== target.ownerDocument) {
      try {
        // may fail on webkit
        return target.ownerDocument.importNode(element, true);
      } catch (e) {// ignore
      }
    }

    return element;
  }
  /**
   * appendTo utility
   */

  /**
   * Append a node to a target element and return the appended node.
   *
   * @param  {SVGElement} element
   * @param  {SVGElement} target
   *
   * @return {SVGElement} the appended node
   */


  function appendTo(element, target) {
    return target.appendChild(ensureImported(element, target));
  }
  /**
   * append utility
   */

  /**
   * Append a node to an element
   *
   * @param  {SVGElement} element
   * @param  {SVGElement} node
   *
   * @return {SVGElement} the element
   */


  function append(target, node) {
    appendTo(node, target);
    return target;
  }
  /**
   * attribute accessor utility
   */


  var LENGTH_ATTR = 2;
  var CSS_PROPERTIES = {
    'alignment-baseline': 1,
    'baseline-shift': 1,
    'clip': 1,
    'clip-path': 1,
    'clip-rule': 1,
    'color': 1,
    'color-interpolation': 1,
    'color-interpolation-filters': 1,
    'color-profile': 1,
    'color-rendering': 1,
    'cursor': 1,
    'direction': 1,
    'display': 1,
    'dominant-baseline': 1,
    'enable-background': 1,
    'fill': 1,
    'fill-opacity': 1,
    'fill-rule': 1,
    'filter': 1,
    'flood-color': 1,
    'flood-opacity': 1,
    'font': 1,
    'font-family': 1,
    'font-size': LENGTH_ATTR,
    'font-size-adjust': 1,
    'font-stretch': 1,
    'font-style': 1,
    'font-variant': 1,
    'font-weight': 1,
    'glyph-orientation-horizontal': 1,
    'glyph-orientation-vertical': 1,
    'image-rendering': 1,
    'kerning': 1,
    'letter-spacing': 1,
    'lighting-color': 1,
    'marker': 1,
    'marker-end': 1,
    'marker-mid': 1,
    'marker-start': 1,
    'mask': 1,
    'opacity': 1,
    'overflow': 1,
    'pointer-events': 1,
    'shape-rendering': 1,
    'stop-color': 1,
    'stop-opacity': 1,
    'stroke': 1,
    'stroke-dasharray': 1,
    'stroke-dashoffset': 1,
    'stroke-linecap': 1,
    'stroke-linejoin': 1,
    'stroke-miterlimit': 1,
    'stroke-opacity': 1,
    'stroke-width': LENGTH_ATTR,
    'text-anchor': 1,
    'text-decoration': 1,
    'text-rendering': 1,
    'unicode-bidi': 1,
    'visibility': 1,
    'word-spacing': 1,
    'writing-mode': 1
  };

  function getAttribute(node, name) {
    if (CSS_PROPERTIES[name]) {
      return node.style[name];
    } else {
      return node.getAttributeNS(null, name);
    }
  }

  function setAttribute(node, name, value) {
    var hyphenated = name.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
    var type = CSS_PROPERTIES[hyphenated];

    if (type) {
      // append pixel unit, unless present
      if (type === LENGTH_ATTR && typeof value === 'number') {
        value = String(value) + 'px';
      }

      node.style[hyphenated] = value;
    } else {
      node.setAttributeNS(null, name, value);
    }
  }

  function setAttributes(node, attrs) {
    var names = Object.keys(attrs),
        i,
        name;

    for (i = 0, name; name = names[i]; i++) {
      setAttribute(node, name, attrs[name]);
    }
  }
  /**
   * Gets or sets raw attributes on a node.
   *
   * @param  {SVGElement} node
   * @param  {Object} [attrs]
   * @param  {String} [name]
   * @param  {String} [value]
   *
   * @return {String}
   */


  function attr$1(node, name, value) {
    if (typeof name === 'string') {
      if (value !== undefined) {
        setAttribute(node, name, value);
      } else {
        return getAttribute(node, name);
      }
    } else {
      setAttributes(node, name);
    }

    return node;
  }
  /**
   * Clear utility
   */


  function index(arr, obj) {
    if (arr.indexOf) {
      return arr.indexOf(obj);
    }

    for (var i = 0; i < arr.length; ++i) {
      if (arr[i] === obj) {
        return i;
      }
    }

    return -1;
  }

  var re$1 = /\s+/;
  var toString$1 = Object.prototype.toString;

  function defined(o) {
    return typeof o !== 'undefined';
  }
  /**
   * Wrap `el` in a `ClassList`.
   *
   * @param {Element} el
   * @return {ClassList}
   * @api public
   */


  function classes$1(el) {
    return new ClassList$1(el);
  }

  function ClassList$1(el) {
    if (!el || !el.nodeType) {
      throw new Error('A DOM element reference is required');
    }

    this.el = el;
    this.list = el.classList;
  }
  /**
   * Add class `name` if not already present.
   *
   * @param {String} name
   * @return {ClassList}
   * @api public
   */


  ClassList$1.prototype.add = function (name) {
    // classList
    if (this.list) {
      this.list.add(name);
      return this;
    } // fallback


    var arr = this.array();
    var i = index(arr, name);

    if (!~i) {
      arr.push(name);
    }

    if (defined(this.el.className.baseVal)) {
      this.el.className.baseVal = arr.join(' ');
    } else {
      this.el.className = arr.join(' ');
    }

    return this;
  };
  /**
   * Remove class `name` when present, or
   * pass a regular expression to remove
   * any which match.
   *
   * @param {String|RegExp} name
   * @return {ClassList}
   * @api public
   */


  ClassList$1.prototype.remove = function (name) {
    if ('[object RegExp]' === toString$1.call(name)) {
      return this.removeMatching(name);
    } // classList


    if (this.list) {
      this.list.remove(name);
      return this;
    } // fallback


    var arr = this.array();
    var i = index(arr, name);

    if (~i) {
      arr.splice(i, 1);
    }

    this.el.className.baseVal = arr.join(' ');
    return this;
  };
  /**
   * Remove all classes matching `re`.
   *
   * @param {RegExp} re
   * @return {ClassList}
   * @api private
   */


  ClassList$1.prototype.removeMatching = function (re) {
    var arr = this.array();

    for (var i = 0; i < arr.length; i++) {
      if (re.test(arr[i])) {
        this.remove(arr[i]);
      }
    }

    return this;
  };
  /**
   * Toggle class `name`, can force state via `force`.
   *
   * For browsers that support classList, but do not support `force` yet,
   * the mistake will be detected and corrected.
   *
   * @param {String} name
   * @param {Boolean} force
   * @return {ClassList}
   * @api public
   */


  ClassList$1.prototype.toggle = function (name, force) {
    // classList
    if (this.list) {
      if (defined(force)) {
        if (force !== this.list.toggle(name, force)) {
          this.list.toggle(name); // toggle again to correct
        }
      } else {
        this.list.toggle(name);
      }

      return this;
    } // fallback


    if (defined(force)) {
      if (!force) {
        this.remove(name);
      } else {
        this.add(name);
      }
    } else {
      if (this.has(name)) {
        this.remove(name);
      } else {
        this.add(name);
      }
    }

    return this;
  };
  /**
   * Return an array of classes.
   *
   * @return {Array}
   * @api public
   */


  ClassList$1.prototype.array = function () {
    var className = this.el.getAttribute('class') || '';
    var str = className.replace(/^\s+|\s+$/g, '');
    var arr = str.split(re$1);

    if ('' === arr[0]) {
      arr.shift();
    }

    return arr;
  };
  /**
   * Check if class `name` is present.
   *
   * @param {String} name
   * @return {ClassList}
   * @api public
   */


  ClassList$1.prototype.has = ClassList$1.prototype.contains = function (name) {
    return this.list ? this.list.contains(name) : !!~index(this.array(), name);
  };

  function remove$1(element) {
    var parent = element.parentNode;

    if (parent) {
      parent.removeChild(element);
    }

    return element;
  }
  /**
   * Clear utility
   */

  /**
   * Removes all children from the given element
   *
   * @param  {DOMElement} element
   * @return {DOMElement} the element (for chaining)
   */


  function clear$1(element) {
    var child;

    while (child = element.firstChild) {
      remove$1(child);
    }

    return element;
  }

  var ns = {
    svg: 'http://www.w3.org/2000/svg'
  };
  /**
   * DOM parsing utility
   */

  var SVG_START = '<svg xmlns="' + ns.svg + '"';

  function parse$1(svg) {
    var unwrap = false; // ensure we import a valid svg document

    if (svg.substring(0, 4) === '<svg') {
      if (svg.indexOf(ns.svg) === -1) {
        svg = SVG_START + svg.substring(4);
      }
    } else {
      // namespace svg
      svg = SVG_START + '>' + svg + '</svg>';
      unwrap = true;
    }

    var parsed = parseDocument(svg);

    if (!unwrap) {
      return parsed;
    }

    var fragment = document.createDocumentFragment();
    var parent = parsed.firstChild;

    while (parent.firstChild) {
      fragment.appendChild(parent.firstChild);
    }

    return fragment;
  }

  function parseDocument(svg) {
    var parser; // parse

    parser = new DOMParser();
    parser.async = false;
    return parser.parseFromString(svg, 'text/xml');
  }
  /**
   * Create utility for SVG elements
   */

  /**
   * Create a specific type from name or SVG markup.
   *
   * @param {String} name the name or markup of the element
   * @param {Object} [attrs] attributes to set on the element
   *
   * @returns {SVGElement}
   */


  function create(name, attrs) {
    var element;

    if (name.charAt(0) === '<') {
      element = parse$1(name).firstChild;
      element = document.importNode(element, true);
    } else {
      element = document.createElementNS(ns.svg, name);
    }

    if (attrs) {
      attr$1(element, attrs);
    }

    return element;
  }
  /**
   * Geometry helpers
   */
  // fake node used to instantiate svg geometry elements


  var node = create('svg');

  function extend(object, props) {
    var i,
        k,
        keys = Object.keys(props);

    for (i = 0; k = keys[i]; i++) {
      object[k] = props[k];
    }

    return object;
  }
  /**
   * Create matrix via args.
   *
   * @example
   *
   * createMatrix({ a: 1, b: 1 });
   * createMatrix();
   * createMatrix(1, 2, 0, 0, 30, 20);
   *
   * @return {SVGMatrix}
   */


  function createMatrix(a, b, c, d, e, f) {
    var matrix = node.createSVGMatrix();

    switch (arguments.length) {
      case 0:
        return matrix;

      case 1:
        return extend(matrix, a);

      case 6:
        return extend(matrix, {
          a: a,
          b: b,
          c: c,
          d: d,
          e: e,
          f: f
        });
    }
  }

  function createTransform(matrix) {
    if (matrix) {
      return node.createSVGTransformFromMatrix(matrix);
    } else {
      return node.createSVGTransform();
    }
  }
  /**
   * Serialization util
   */


  var TEXT_ENTITIES = /([&<>]{1})/g;
  var ATTR_ENTITIES = /([\n\r"]{1})/g;
  var ENTITY_REPLACEMENT = {
    '&': '&amp;',
    '<': '&lt;',
    '>': '&gt;',
    '"': '\''
  };

  function escape$1(str, pattern) {
    function replaceFn(match, entity) {
      return ENTITY_REPLACEMENT[entity] || entity;
    }

    return str.replace(pattern, replaceFn);
  }

  function serialize(node, output) {
    var i, len, attrMap, attrNode, childNodes;

    switch (node.nodeType) {
      // TEXT
      case 3:
        // replace special XML characters
        output.push(escape$1(node.textContent, TEXT_ENTITIES));
        break;
      // ELEMENT

      case 1:
        output.push('<', node.tagName);

        if (node.hasAttributes()) {
          attrMap = node.attributes;

          for (i = 0, len = attrMap.length; i < len; ++i) {
            attrNode = attrMap.item(i);
            output.push(' ', attrNode.name, '="', escape$1(attrNode.value, ATTR_ENTITIES), '"');
          }
        }

        if (node.hasChildNodes()) {
          output.push('>');
          childNodes = node.childNodes;

          for (i = 0, len = childNodes.length; i < len; ++i) {
            serialize(childNodes.item(i), output);
          }

          output.push('</', node.tagName, '>');
        } else {
          output.push('/>');
        }

        break;
      // COMMENT

      case 8:
        output.push('<!--', escape$1(node.nodeValue, TEXT_ENTITIES), '-->');
        break;
      // CDATA

      case 4:
        output.push('<![CDATA[', node.nodeValue, ']]>');
        break;

      default:
        throw new Error('unable to handle node ' + node.nodeType);
    }

    return output;
  }
  /**
   * innerHTML like functionality for SVG elements.
   * based on innerSVG (https://code.google.com/p/innersvg)
   */


  function set(element, svg) {
    var parsed = parse$1(svg); // clear element contents

    clear$1(element);

    if (!svg) {
      return;
    }

    if (!isFragment(parsed)) {
      // extract <svg> from parsed document
      parsed = parsed.documentElement;
    }

    var nodes = slice$1(parsed.childNodes); // import + append each node

    for (var i = 0; i < nodes.length; i++) {
      appendTo(nodes[i], element);
    }
  }

  function get(element) {
    var child = element.firstChild,
        output = [];

    while (child) {
      serialize(child, output);
      child = child.nextSibling;
    }

    return output.join('');
  }

  function isFragment(node) {
    return node.nodeName === '#document-fragment';
  }

  function innerSVG(element, svg) {
    if (svg !== undefined) {
      try {
        set(element, svg);
      } catch (e) {
        throw new Error('error parsing SVG: ' + e.message);
      }

      return element;
    } else {
      return get(element);
    }
  }

  function slice$1(arr) {
    return Array.prototype.slice.call(arr);
  }
  /**
   * transform accessor utility
   */


  function wrapMatrix(transformList, transform) {
    if (transform instanceof SVGMatrix) {
      return transformList.createSVGTransformFromMatrix(transform);
    }

    return transform;
  }

  function setTransforms(transformList, transforms) {
    var i, t;
    transformList.clear();

    for (i = 0; t = transforms[i]; i++) {
      transformList.appendItem(wrapMatrix(transformList, t));
    }
  }
  /**
   * Get or set the transforms on the given node.
   *
   * @param {SVGElement} node
   * @param  {SVGTransform|SVGMatrix|Array<SVGTransform|SVGMatrix>} [transforms]
   *
   * @return {SVGTransform} the consolidated transform
   */


  function transform(node, transforms) {
    var transformList = node.transform.baseVal;

    if (transforms) {
      if (!Array.isArray(transforms)) {
        transforms = [transforms];
      }

      setTransforms(transformList, transforms);
    }

    return transformList.consolidate();
  }

  function componentsToPath(elements) {
    return elements.join(',').replace(/,?([A-z]),?/g, '$1');
  }
  function toSVGPoints(points) {
    var result = '';

    for (var i = 0, p; p = points[i]; i++) {
      result += p.x + ',' + p.y + ' ';
    }

    return result;
  }
  function createLine(points, attrs) {
    var line = create('polyline');
    attr$1(line, {
      points: toSVGPoints(points)
    });

    if (attrs) {
      attr$1(line, attrs);
    }

    return line;
  }
  function updateLine(gfx, points) {
    attr$1(gfx, {
      points: toSVGPoints(points)
    });
    return gfx;
  }

  /**
   * Returns the surrounding bbox for all elements in
   * the array or the element primitive.
   *
   * @param {Array<djs.model.Shape>|djs.model.Shape} elements
   * @param {boolean} stopRecursion
   */

  function getBBox(elements, stopRecursion) {
    stopRecursion = !!stopRecursion;

    if (!isArray(elements)) {
      elements = [elements];
    }

    var minX, minY, maxX, maxY;
    forEach(elements, function (element) {
      // If element is a connection the bbox must be computed first
      var bbox = element;

      if (element.waypoints && !stopRecursion) {
        bbox = getBBox(element.waypoints, true);
      }

      var x = bbox.x,
          y = bbox.y,
          height = bbox.height || 0,
          width = bbox.width || 0;

      if (x < minX || minX === undefined) {
        minX = x;
      }

      if (y < minY || minY === undefined) {
        minY = y;
      }

      if (x + width > maxX || maxX === undefined) {
        maxX = x + width;
      }

      if (y + height > maxY || maxY === undefined) {
        maxY = y + height;
      }
    });
    return {
      x: minX,
      y: minY,
      height: maxY - minY,
      width: maxX - minX
    };
  }
  function getType(element) {
    if ('waypoints' in element) {
      return 'connection';
    }

    if ('x' in element) {
      return 'shape';
    }

    return 'root';
  }
  function isFrameElement(element) {
    return !!(element && element.isFrame);
  } // helpers ///////////////////////////////

  // so that it only kicks in if noone else could render

  var DEFAULT_RENDER_PRIORITY$1 = 1;
  /**
   * The default renderer used for shapes and connections.
   *
   * @param {EventBus} eventBus
   * @param {Styles} styles
   */

  function DefaultRenderer(eventBus, styles) {
    //
    BaseRenderer.call(this, eventBus, DEFAULT_RENDER_PRIORITY$1);
    this.CONNECTION_STYLE = styles.style(['no-fill'], {
      strokeWidth: 5,
      stroke: 'fuchsia'
    });
    this.SHAPE_STYLE = styles.style({
      fill: 'white',
      stroke: 'fuchsia',
      strokeWidth: 2
    });
    this.FRAME_STYLE = styles.style(['no-fill'], {
      stroke: 'fuchsia',
      strokeDasharray: 4,
      strokeWidth: 2
    });
  }
  inherits_browser(DefaultRenderer, BaseRenderer);

  DefaultRenderer.prototype.canRender = function () {
    return true;
  };

  DefaultRenderer.prototype.drawShape = function drawShape(visuals, element) {
    var rect = create('rect');
    attr$1(rect, {
      x: 0,
      y: 0,
      width: element.width || 0,
      height: element.height || 0
    });

    if (isFrameElement(element)) {
      attr$1(rect, this.FRAME_STYLE);
    } else {
      attr$1(rect, this.SHAPE_STYLE);
    }

    append(visuals, rect);
    return rect;
  };

  DefaultRenderer.prototype.drawConnection = function drawConnection(visuals, connection) {
    var line = createLine(connection.waypoints, this.CONNECTION_STYLE);
    append(visuals, line);
    return line;
  };

  DefaultRenderer.prototype.getShapePath = function getShapePath(shape) {
    var x = shape.x,
        y = shape.y,
        width = shape.width,
        height = shape.height;
    var shapePath = [['M', x, y], ['l', width, 0], ['l', 0, height], ['l', -width, 0], ['z']];
    return componentsToPath(shapePath);
  };

  DefaultRenderer.prototype.getConnectionPath = function getConnectionPath(connection) {
    var waypoints = connection.waypoints;
    var idx,
        point,
        connectionPath = [];

    for (idx = 0; point = waypoints[idx]; idx++) {
      // take invisible docking into account
      // when creating the path
      point = point.original || point;
      connectionPath.push([idx === 0 ? 'M' : 'L', point.x, point.y]);
    }

    return componentsToPath(connectionPath);
  };

  DefaultRenderer.$inject = ['eventBus', 'styles'];

  /**
   * A component that manages shape styles
   */

  function Styles() {
    var defaultTraits = {
      'no-fill': {
        fill: 'none'
      },
      'no-border': {
        strokeOpacity: 0.0
      },
      'no-events': {
        pointerEvents: 'none'
      }
    };
    var self = this;
    /**
     * Builds a style definition from a className, a list of traits and an object of additional attributes.
     *
     * @param  {string} className
     * @param  {Array<string>} traits
     * @param  {Object} additionalAttrs
     *
     * @return {Object} the style defintion
     */

    this.cls = function (className, traits, additionalAttrs) {
      var attrs = this.style(traits, additionalAttrs);
      return assign(attrs, {
        'class': className
      });
    };
    /**
     * Builds a style definition from a list of traits and an object of additional attributes.
     *
     * @param  {Array<string>} traits
     * @param  {Object} additionalAttrs
     *
     * @return {Object} the style defintion
     */


    this.style = function (traits, additionalAttrs) {
      if (!isArray(traits) && !additionalAttrs) {
        additionalAttrs = traits;
        traits = [];
      }

      var attrs = reduce(traits, function (attrs, t) {
        return assign(attrs, defaultTraits[t] || {});
      }, {});
      return additionalAttrs ? assign(attrs, additionalAttrs) : attrs;
    };

    this.computeStyle = function (custom, traits, defaultStyles) {
      if (!isArray(traits)) {
        defaultStyles = traits;
        traits = [];
      }

      return self.style(traits || [], assign({}, defaultStyles, custom || {}));
    };
  }

  var DrawModule = {
    __init__: ['defaultRenderer'],
    defaultRenderer: ['type', DefaultRenderer],
    styles: ['type', Styles]
  };

  /**
   * Failsafe remove an element from a collection
   *
   * @param  {Array<Object>} [collection]
   * @param  {Object} [element]
   *
   * @return {number} the previous index of the element
   */
  function remove$2(collection, element) {
    if (!collection || !element) {
      return -1;
    }

    var idx = collection.indexOf(element);

    if (idx !== -1) {
      collection.splice(idx, 1);
    }

    return idx;
  }
  /**
   * Fail save add an element to the given connection, ensuring
   * it does not yet exist.
   *
   * @param {Array<Object>} collection
   * @param {Object} element
   * @param {number} idx
   */

  function add(collection, element, idx) {
    if (!collection || !element) {
      return;
    }

    if (typeof idx !== 'number') {
      idx = -1;
    }

    var currentIdx = collection.indexOf(element);

    if (currentIdx !== -1) {
      if (currentIdx === idx) {
        // nothing to do, position has not changed
        return;
      } else {
        if (idx !== -1) {
          // remove from current position
          collection.splice(currentIdx, 1);
        } else {
          // already exists in collection
          return;
        }
      }
    }

    if (idx !== -1) {
      // insert at specified position
      collection.splice(idx, 0, element);
    } else {
      // push to end
      collection.push(element);
    }
  }

  function round(number, resolution) {
    return Math.round(number * resolution) / resolution;
  }

  function ensurePx(number) {
    return isNumber(number) ? number + 'px' : number;
  }
  /**
   * Creates a HTML container element for a SVG element with
   * the given configuration
   *
   * @param  {Object} options
   * @return {HTMLElement} the container element
   */


  function createContainer(options) {
    options = assign({}, {
      width: '100%',
      height: '100%'
    }, options);
    var container = options.container || document.body; // create a <div> around the svg element with the respective size
    // this way we can always get the correct container size
    // (this is impossible for <svg> elements at the moment)

    var parent = document.createElement('div');
    parent.setAttribute('class', 'djs-container');
    assign(parent.style, {
      position: 'relative',
      overflow: 'hidden',
      width: ensurePx(options.width),
      height: ensurePx(options.height)
    });
    container.appendChild(parent);
    return parent;
  }

  function createGroup(parent, cls, childIndex) {
    var group = create('g');
    classes$1(group).add(cls);
    var index = childIndex !== undefined ? childIndex : parent.childNodes.length - 1; // must ensure second argument is node or _null_
    // cf. https://developer.mozilla.org/en-US/docs/Web/API/Node/insertBefore

    parent.insertBefore(group, parent.childNodes[index] || null);
    return group;
  }

  var BASE_LAYER = 'base';
  var REQUIRED_MODEL_ATTRS = {
    shape: ['x', 'y', 'width', 'height'],
    connection: ['waypoints']
  };
  /**
   * The main drawing canvas.
   *
   * @class
   * @constructor
   *
   * @emits Canvas#canvas.init
   *
   * @param {Object} config
   * @param {EventBus} eventBus
   * @param {GraphicsFactory} graphicsFactory
   * @param {ElementRegistry} elementRegistry
   */

  function Canvas(config, eventBus, graphicsFactory, elementRegistry) {
    this._eventBus = eventBus;
    this._elementRegistry = elementRegistry;
    this._graphicsFactory = graphicsFactory;

    this._init(config || {});
  }
  Canvas.$inject = ['config.canvas', 'eventBus', 'graphicsFactory', 'elementRegistry'];

  Canvas.prototype._init = function (config) {
    var eventBus = this._eventBus; // Creates a <svg> element that is wrapped into a <div>.
    // This way we are always able to correctly figure out the size of the svg element
    // by querying the parent node.
    //
    // (It is not possible to get the size of a svg element cross browser @ 2014-04-01)
    //
    // <div class="djs-container" style="width: {desired-width}, height: {desired-height}">
    //   <svg width="100%" height="100%">
    //    ...
    //   </svg>
    // </div>
    // html container

    var container = this._container = createContainer(config);
    var svg = this._svg = create('svg');
    attr$1(svg, {
      width: '100%',
      height: '100%'
    });
    append(container, svg);
    var viewport = this._viewport = createGroup(svg, 'viewport');
    this._layers = {}; // debounce canvas.viewbox.changed events
    // for smoother diagram interaction

    if (config.deferUpdate !== false) {
      this._viewboxChanged = debounce(bind(this._viewboxChanged, this), 300);
    }

    eventBus.on('diagram.init', function () {
      /**
       * An event indicating that the canvas is ready to be drawn on.
       *
       * @memberOf Canvas
       *
       * @event canvas.init
       *
       * @type {Object}
       * @property {SVGElement} svg the created svg element
       * @property {SVGElement} viewport the direct parent of diagram elements and shapes
       */
      eventBus.fire('canvas.init', {
        svg: svg,
        viewport: viewport
      });
    }, this); // reset viewbox on shape changes to
    // recompute the viewbox

    eventBus.on(['shape.added', 'connection.added', 'shape.removed', 'connection.removed', 'elements.changed'], function () {
      delete this._cachedViewbox;
    }, this);
    eventBus.on('diagram.destroy', 500, this._destroy, this);
    eventBus.on('diagram.clear', 500, this._clear, this);
  };

  Canvas.prototype._destroy = function (emit) {
    this._eventBus.fire('canvas.destroy', {
      svg: this._svg,
      viewport: this._viewport
    });

    var parent = this._container.parentNode;

    if (parent) {
      parent.removeChild(this._container);
    }

    delete this._svg;
    delete this._container;
    delete this._layers;
    delete this._rootElement;
    delete this._viewport;
  };

  Canvas.prototype._clear = function () {
    var self = this;

    var allElements = this._elementRegistry.getAll(); // remove all elements


    allElements.forEach(function (element) {
      var type = getType(element);

      if (type === 'root') {
        self.setRootElement(null, true);
      } else {
        self._removeElement(element, type);
      }
    }); // force recomputation of view box

    delete this._cachedViewbox;
  };
  /**
   * Returns the default layer on which
   * all elements are drawn.
   *
   * @returns {SVGElement}
   */


  Canvas.prototype.getDefaultLayer = function () {
    return this.getLayer(BASE_LAYER, 0);
  };
  /**
   * Returns a layer that is used to draw elements
   * or annotations on it.
   *
   * Non-existing layers retrieved through this method
   * will be created. During creation, the optional index
   * may be used to create layers below or above existing layers.
   * A layer with a certain index is always created above all
   * existing layers with the same index.
   *
   * @param {string} name
   * @param {number} index
   *
   * @returns {SVGElement}
   */


  Canvas.prototype.getLayer = function (name, index) {
    if (!name) {
      throw new Error('must specify a name');
    }

    var layer = this._layers[name];

    if (!layer) {
      layer = this._layers[name] = this._createLayer(name, index);
    } // throw an error if layer creation / retrival is
    // requested on different index


    if (typeof index !== 'undefined' && layer.index !== index) {
      throw new Error('layer <' + name + '> already created at index <' + index + '>');
    }

    return layer.group;
  };
  /**
   * Creates a given layer and returns it.
   *
   * @param {string} name
   * @param {number} [index=0]
   *
   * @return {Object} layer descriptor with { index, group: SVGGroup }
   */


  Canvas.prototype._createLayer = function (name, index) {
    if (!index) {
      index = 0;
    }

    var childIndex = reduce(this._layers, function (childIndex, layer) {
      if (index >= layer.index) {
        childIndex++;
      }

      return childIndex;
    }, 0);
    return {
      group: createGroup(this._viewport, 'layer-' + name, childIndex),
      index: index
    };
  };
  /**
   * Returns the html element that encloses the
   * drawing canvas.
   *
   * @return {DOMNode}
   */


  Canvas.prototype.getContainer = function () {
    return this._container;
  }; // markers //////////////////////


  Canvas.prototype._updateMarker = function (element, marker, add) {
    var container;

    if (!element.id) {
      element = this._elementRegistry.get(element);
    } // we need to access all


    container = this._elementRegistry._elements[element.id];

    if (!container) {
      return;
    }

    forEach([container.gfx, container.secondaryGfx], function (gfx) {
      if (gfx) {
        // invoke either addClass or removeClass based on mode
        if (add) {
          classes$1(gfx).add(marker);
        } else {
          classes$1(gfx).remove(marker);
        }
      }
    });
    /**
     * An event indicating that a marker has been updated for an element
     *
     * @event element.marker.update
     * @type {Object}
     * @property {djs.model.Element} element the shape
     * @property {Object} gfx the graphical representation of the shape
     * @property {string} marker
     * @property {boolean} add true if the marker was added, false if it got removed
     */

    this._eventBus.fire('element.marker.update', {
      element: element,
      gfx: container.gfx,
      marker: marker,
      add: !!add
    });
  };
  /**
   * Adds a marker to an element (basically a css class).
   *
   * Fires the element.marker.update event, making it possible to
   * integrate extension into the marker life-cycle, too.
   *
   * @example
   * canvas.addMarker('foo', 'some-marker');
   *
   * var fooGfx = canvas.getGraphics('foo');
   *
   * fooGfx; // <g class="... some-marker"> ... </g>
   *
   * @param {string|djs.model.Base} element
   * @param {string} marker
   */


  Canvas.prototype.addMarker = function (element, marker) {
    this._updateMarker(element, marker, true);
  };
  /**
   * Remove a marker from an element.
   *
   * Fires the element.marker.update event, making it possible to
   * integrate extension into the marker life-cycle, too.
   *
   * @param  {string|djs.model.Base} element
   * @param  {string} marker
   */


  Canvas.prototype.removeMarker = function (element, marker) {
    this._updateMarker(element, marker, false);
  };
  /**
   * Check the existence of a marker on element.
   *
   * @param  {string|djs.model.Base} element
   * @param  {string} marker
   */


  Canvas.prototype.hasMarker = function (element, marker) {
    if (!element.id) {
      element = this._elementRegistry.get(element);
    }

    var gfx = this.getGraphics(element);
    return classes$1(gfx).has(marker);
  };
  /**
   * Toggles a marker on an element.
   *
   * Fires the element.marker.update event, making it possible to
   * integrate extension into the marker life-cycle, too.
   *
   * @param  {string|djs.model.Base} element
   * @param  {string} marker
   */


  Canvas.prototype.toggleMarker = function (element, marker) {
    if (this.hasMarker(element, marker)) {
      this.removeMarker(element, marker);
    } else {
      this.addMarker(element, marker);
    }
  };

  Canvas.prototype.getRootElement = function () {
    if (!this._rootElement) {
      this.setRootElement({
        id: '__implicitroot',
        children: []
      });
    }

    return this._rootElement;
  }; // root element handling //////////////////////

  /**
   * Sets a given element as the new root element for the canvas
   * and returns the new root element.
   *
   * @param {Object|djs.model.Root} element
   * @param {boolean} [override] whether to override the current root element, if any
   *
   * @return {Object|djs.model.Root} new root element
   */


  Canvas.prototype.setRootElement = function (element, override) {
    if (element) {
      this._ensureValid('root', element);
    }

    var currentRoot = this._rootElement,
        elementRegistry = this._elementRegistry,
        eventBus = this._eventBus;

    if (currentRoot) {
      if (!override) {
        throw new Error('rootElement already set, need to specify override');
      } // simulate element remove event sequence


      eventBus.fire('root.remove', {
        element: currentRoot
      });
      eventBus.fire('root.removed', {
        element: currentRoot
      });
      elementRegistry.remove(currentRoot);
    }

    if (element) {
      var gfx = this.getDefaultLayer(); // resemble element add event sequence

      eventBus.fire('root.add', {
        element: element
      });
      elementRegistry.add(element, gfx, this._svg);
      eventBus.fire('root.added', {
        element: element,
        gfx: gfx
      });
    }

    this._rootElement = element;
    return element;
  }; // add functionality //////////////////////


  Canvas.prototype._ensureValid = function (type, element) {
    if (!element.id) {
      throw new Error('element must have an id');
    }

    if (this._elementRegistry.get(element.id)) {
      throw new Error('element with id ' + element.id + ' already exists');
    }

    var requiredAttrs = REQUIRED_MODEL_ATTRS[type];
    var valid = every(requiredAttrs, function (attr) {
      return typeof element[attr] !== 'undefined';
    });

    if (!valid) {
      throw new Error('must supply { ' + requiredAttrs.join(', ') + ' } with ' + type);
    }
  };

  Canvas.prototype._setParent = function (element, parent, parentIndex) {
    add(parent.children, element, parentIndex);
    element.parent = parent;
  };
  /**
   * Adds an element to the canvas.
   *
   * This wires the parent <-> child relationship between the element and
   * a explicitly specified parent or an implicit root element.
   *
   * During add it emits the events
   *
   *  * <{type}.add> (element, parent)
   *  * <{type}.added> (element, gfx)
   *
   * Extensions may hook into these events to perform their magic.
   *
   * @param {string} type
   * @param {Object|djs.model.Base} element
   * @param {Object|djs.model.Base} [parent]
   * @param {number} [parentIndex]
   *
   * @return {Object|djs.model.Base} the added element
   */


  Canvas.prototype._addElement = function (type, element, parent, parentIndex) {
    parent = parent || this.getRootElement();
    var eventBus = this._eventBus,
        graphicsFactory = this._graphicsFactory;

    this._ensureValid(type, element);

    eventBus.fire(type + '.add', {
      element: element,
      parent: parent
    });

    this._setParent(element, parent, parentIndex); // create graphics


    var gfx = graphicsFactory.create(type, element, parentIndex);

    this._elementRegistry.add(element, gfx); // update its visual


    graphicsFactory.update(type, element, gfx);
    eventBus.fire(type + '.added', {
      element: element,
      gfx: gfx
    });
    return element;
  };
  /**
   * Adds a shape to the canvas
   *
   * @param {Object|djs.model.Shape} shape to add to the diagram
   * @param {djs.model.Base} [parent]
   * @param {number} [parentIndex]
   *
   * @return {djs.model.Shape} the added shape
   */


  Canvas.prototype.addShape = function (shape, parent, parentIndex) {
    return this._addElement('shape', shape, parent, parentIndex);
  };
  /**
   * Adds a connection to the canvas
   *
   * @param {Object|djs.model.Connection} connection to add to the diagram
   * @param {djs.model.Base} [parent]
   * @param {number} [parentIndex]
   *
   * @return {djs.model.Connection} the added connection
   */


  Canvas.prototype.addConnection = function (connection, parent, parentIndex) {
    return this._addElement('connection', connection, parent, parentIndex);
  };
  /**
   * Internal remove element
   */


  Canvas.prototype._removeElement = function (element, type) {
    var elementRegistry = this._elementRegistry,
        graphicsFactory = this._graphicsFactory,
        eventBus = this._eventBus;
    element = elementRegistry.get(element.id || element);

    if (!element) {
      // element was removed already
      return;
    }

    eventBus.fire(type + '.remove', {
      element: element
    });
    graphicsFactory.remove(element); // unset parent <-> child relationship

    remove$2(element.parent && element.parent.children, element);
    element.parent = null;
    eventBus.fire(type + '.removed', {
      element: element
    });
    elementRegistry.remove(element);
    return element;
  };
  /**
   * Removes a shape from the canvas
   *
   * @param {string|djs.model.Shape} shape or shape id to be removed
   *
   * @return {djs.model.Shape} the removed shape
   */


  Canvas.prototype.removeShape = function (shape) {
    /**
     * An event indicating that a shape is about to be removed from the canvas.
     *
     * @memberOf Canvas
     *
     * @event shape.remove
     * @type {Object}
     * @property {djs.model.Shape} element the shape descriptor
     * @property {Object} gfx the graphical representation of the shape
     */

    /**
     * An event indicating that a shape has been removed from the canvas.
     *
     * @memberOf Canvas
     *
     * @event shape.removed
     * @type {Object}
     * @property {djs.model.Shape} element the shape descriptor
     * @property {Object} gfx the graphical representation of the shape
     */
    return this._removeElement(shape, 'shape');
  };
  /**
   * Removes a connection from the canvas
   *
   * @param {string|djs.model.Connection} connection or connection id to be removed
   *
   * @return {djs.model.Connection} the removed connection
   */


  Canvas.prototype.removeConnection = function (connection) {
    /**
     * An event indicating that a connection is about to be removed from the canvas.
     *
     * @memberOf Canvas
     *
     * @event connection.remove
     * @type {Object}
     * @property {djs.model.Connection} element the connection descriptor
     * @property {Object} gfx the graphical representation of the connection
     */

    /**
     * An event indicating that a connection has been removed from the canvas.
     *
     * @memberOf Canvas
     *
     * @event connection.removed
     * @type {Object}
     * @property {djs.model.Connection} element the connection descriptor
     * @property {Object} gfx the graphical representation of the connection
     */
    return this._removeElement(connection, 'connection');
  };
  /**
   * Return the graphical object underlaying a certain diagram element
   *
   * @param {string|djs.model.Base} element descriptor of the element
   * @param {boolean} [secondary=false] whether to return the secondary connected element
   *
   * @return {SVGElement}
   */


  Canvas.prototype.getGraphics = function (element, secondary) {
    return this._elementRegistry.getGraphics(element, secondary);
  };
  /**
   * Perform a viewbox update via a given change function.
   *
   * @param {Function} changeFn
   */


  Canvas.prototype._changeViewbox = function (changeFn) {
    // notify others of the upcoming viewbox change
    this._eventBus.fire('canvas.viewbox.changing'); // perform actual change


    changeFn.apply(this); // reset the cached viewbox so that
    // a new get operation on viewbox or zoom
    // triggers a viewbox re-computation

    this._cachedViewbox = null; // notify others of the change; this step
    // may or may not be debounced

    this._viewboxChanged();
  };

  Canvas.prototype._viewboxChanged = function () {
    this._eventBus.fire('canvas.viewbox.changed', {
      viewbox: this.viewbox()
    });
  };
  /**
   * Gets or sets the view box of the canvas, i.e. the
   * area that is currently displayed.
   *
   * The getter may return a cached viewbox (if it is currently
   * changing). To force a recomputation, pass `false` as the first argument.
   *
   * @example
   *
   * canvas.viewbox({ x: 100, y: 100, width: 500, height: 500 })
   *
   * // sets the visible area of the diagram to (100|100) -> (600|100)
   * // and and scales it according to the diagram width
   *
   * var viewbox = canvas.viewbox(); // pass `false` to force recomputing the box.
   *
   * console.log(viewbox);
   * // {
   * //   inner: Dimensions,
   * //   outer: Dimensions,
   * //   scale,
   * //   x, y,
   * //   width, height
   * // }
   *
   * // if the current diagram is zoomed and scrolled, you may reset it to the
   * // default zoom via this method, too:
   *
   * var zoomedAndScrolledViewbox = canvas.viewbox();
   *
   * canvas.viewbox({
   *   x: 0,
   *   y: 0,
   *   width: zoomedAndScrolledViewbox.outer.width,
   *   height: zoomedAndScrolledViewbox.outer.height
   * });
   *
   * @param  {Object} [box] the new view box to set
   * @param  {number} box.x the top left X coordinate of the canvas visible in view box
   * @param  {number} box.y the top left Y coordinate of the canvas visible in view box
   * @param  {number} box.width the visible width
   * @param  {number} box.height
   *
   * @return {Object} the current view box
   */


  Canvas.prototype.viewbox = function (box) {
    if (box === undefined && this._cachedViewbox) {
      return this._cachedViewbox;
    }

    var viewport = this._viewport,
        innerBox,
        outerBox = this.getSize(),
        matrix,
        transform$1,
        scale,
        x,
        y;

    if (!box) {
      // compute the inner box based on the
      // diagrams default layer. This allows us to exclude
      // external components, such as overlays
      innerBox = this.getDefaultLayer().getBBox();
      transform$1 = transform(viewport);
      matrix = transform$1 ? transform$1.matrix : createMatrix();
      scale = round(matrix.a, 1000);
      x = round(-matrix.e || 0, 1000);
      y = round(-matrix.f || 0, 1000);
      box = this._cachedViewbox = {
        x: x ? x / scale : 0,
        y: y ? y / scale : 0,
        width: outerBox.width / scale,
        height: outerBox.height / scale,
        scale: scale,
        inner: {
          width: innerBox.width,
          height: innerBox.height,
          x: innerBox.x,
          y: innerBox.y
        },
        outer: outerBox
      };
      return box;
    } else {
      this._changeViewbox(function () {
        scale = Math.min(outerBox.width / box.width, outerBox.height / box.height);

        var matrix = this._svg.createSVGMatrix().scale(scale).translate(-box.x, -box.y);

        transform(viewport, matrix);
      });
    }

    return box;
  };
  /**
   * Gets or sets the scroll of the canvas.
   *
   * @param {Object} [delta] the new scroll to apply.
   *
   * @param {number} [delta.dx]
   * @param {number} [delta.dy]
   */


  Canvas.prototype.scroll = function (delta) {
    var node = this._viewport;
    var matrix = node.getCTM();

    if (delta) {
      this._changeViewbox(function () {
        delta = assign({
          dx: 0,
          dy: 0
        }, delta || {});
        matrix = this._svg.createSVGMatrix().translate(delta.dx, delta.dy).multiply(matrix);
        setCTM(node, matrix);
      });
    }

    return {
      x: matrix.e,
      y: matrix.f
    };
  };
  /**
   * Gets or sets the current zoom of the canvas, optionally zooming
   * to the specified position.
   *
   * The getter may return a cached zoom level. Call it with `false` as
   * the first argument to force recomputation of the current level.
   *
   * @param {string|number} [newScale] the new zoom level, either a number, i.e. 0.9,
   *                                   or `fit-viewport` to adjust the size to fit the current viewport
   * @param {string|Point} [center] the reference point { x: .., y: ..} to zoom to, 'auto' to zoom into mid or null
   *
   * @return {number} the current scale
   */


  Canvas.prototype.zoom = function (newScale, center) {
    if (!newScale) {
      return this.viewbox(newScale).scale;
    }

    if (newScale === 'fit-viewport') {
      return this._fitViewport(center);
    }

    var outer, matrix;

    this._changeViewbox(function () {
      if (_typeof(center) !== 'object') {
        outer = this.viewbox().outer;
        center = {
          x: outer.width / 2,
          y: outer.height / 2
        };
      }

      matrix = this._setZoom(newScale, center);
    });

    return round(matrix.a, 1000);
  };

  function setCTM(node, m) {
    var mstr = 'matrix(' + m.a + ',' + m.b + ',' + m.c + ',' + m.d + ',' + m.e + ',' + m.f + ')';
    node.setAttribute('transform', mstr);
  }

  Canvas.prototype._fitViewport = function (center) {
    var vbox = this.viewbox(),
        outer = vbox.outer,
        inner = vbox.inner,
        newScale,
        newViewbox; // display the complete diagram without zooming in.
    // instead of relying on internal zoom, we perform a
    // hard reset on the canvas viewbox to realize this
    //
    // if diagram does not need to be zoomed in, we focus it around
    // the diagram origin instead

    if (inner.x >= 0 && inner.y >= 0 && inner.x + inner.width <= outer.width && inner.y + inner.height <= outer.height && !center) {
      newViewbox = {
        x: 0,
        y: 0,
        width: Math.max(inner.width + inner.x, outer.width),
        height: Math.max(inner.height + inner.y, outer.height)
      };
    } else {
      newScale = Math.min(1, outer.width / inner.width, outer.height / inner.height);
      newViewbox = {
        x: inner.x + (center ? inner.width / 2 - outer.width / newScale / 2 : 0),
        y: inner.y + (center ? inner.height / 2 - outer.height / newScale / 2 : 0),
        width: outer.width / newScale,
        height: outer.height / newScale
      };
    }

    this.viewbox(newViewbox);
    return this.viewbox(false).scale;
  };

  Canvas.prototype._setZoom = function (scale, center) {
    var svg = this._svg,
        viewport = this._viewport;
    var matrix = svg.createSVGMatrix();
    var point = svg.createSVGPoint();
    var centerPoint, originalPoint, currentMatrix, scaleMatrix, newMatrix;
    currentMatrix = viewport.getCTM();
    var currentScale = currentMatrix.a;

    if (center) {
      centerPoint = assign(point, center); // revert applied viewport transformations

      originalPoint = centerPoint.matrixTransform(currentMatrix.inverse()); // create scale matrix

      scaleMatrix = matrix.translate(originalPoint.x, originalPoint.y).scale(1 / currentScale * scale).translate(-originalPoint.x, -originalPoint.y);
      newMatrix = currentMatrix.multiply(scaleMatrix);
    } else {
      newMatrix = matrix.scale(scale);
    }

    setCTM(this._viewport, newMatrix);
    return newMatrix;
  };
  /**
   * Returns the size of the canvas
   *
   * @return {Dimensions}
   */


  Canvas.prototype.getSize = function () {
    return {
      width: this._container.clientWidth,
      height: this._container.clientHeight
    };
  };
  /**
   * Return the absolute bounding box for the given element
   *
   * The absolute bounding box may be used to display overlays in the
   * callers (browser) coordinate system rather than the zoomed in/out
   * canvas coordinates.
   *
   * @param  {ElementDescriptor} element
   * @return {Bounds} the absolute bounding box
   */


  Canvas.prototype.getAbsoluteBBox = function (element) {
    var vbox = this.viewbox();
    var bbox; // connection
    // use svg bbox

    if (element.waypoints) {
      var gfx = this.getGraphics(element);
      bbox = gfx.getBBox();
    } // shapes
    // use data
    else {
        bbox = element;
      }

    var x = bbox.x * vbox.scale - vbox.x * vbox.scale;
    var y = bbox.y * vbox.scale - vbox.y * vbox.scale;
    var width = bbox.width * vbox.scale;
    var height = bbox.height * vbox.scale;
    return {
      x: x,
      y: y,
      width: width,
      height: height
    };
  };
  /**
   * Fires an event in order other modules can react to the
   * canvas resizing
   */


  Canvas.prototype.resized = function () {
    // force recomputation of view box
    delete this._cachedViewbox;

    this._eventBus.fire('canvas.resized');
  };

  var ELEMENT_ID = 'data-element-id';
  /**
   * @class
   *
   * A registry that keeps track of all shapes in the diagram.
   */

  function ElementRegistry(eventBus) {
    this._elements = {};
    this._eventBus = eventBus;
  }
  ElementRegistry.$inject = ['eventBus'];
  /**
   * Register a pair of (element, gfx, (secondaryGfx)).
   *
   * @param {djs.model.Base} element
   * @param {SVGElement} gfx
   * @param {SVGElement} [secondaryGfx] optional other element to register, too
   */

  ElementRegistry.prototype.add = function (element, gfx, secondaryGfx) {
    var id = element.id;

    this._validateId(id); // associate dom node with element


    attr$1(gfx, ELEMENT_ID, id);

    if (secondaryGfx) {
      attr$1(secondaryGfx, ELEMENT_ID, id);
    }

    this._elements[id] = {
      element: element,
      gfx: gfx,
      secondaryGfx: secondaryGfx
    };
  };
  /**
   * Removes an element from the registry.
   *
   * @param {djs.model.Base} element
   */


  ElementRegistry.prototype.remove = function (element) {
    var elements = this._elements,
        id = element.id || element,
        container = id && elements[id];

    if (container) {
      // unset element id on gfx
      attr$1(container.gfx, ELEMENT_ID, '');

      if (container.secondaryGfx) {
        attr$1(container.secondaryGfx, ELEMENT_ID, '');
      }

      delete elements[id];
    }
  };
  /**
   * Update the id of an element
   *
   * @param {djs.model.Base} element
   * @param {string} newId
   */


  ElementRegistry.prototype.updateId = function (element, newId) {
    this._validateId(newId);

    if (typeof element === 'string') {
      element = this.get(element);
    }

    this._eventBus.fire('element.updateId', {
      element: element,
      newId: newId
    });

    var gfx = this.getGraphics(element),
        secondaryGfx = this.getGraphics(element, true);
    this.remove(element);
    element.id = newId;
    this.add(element, gfx, secondaryGfx);
  };
  /**
   * Return the model element for a given id or graphics.
   *
   * @example
   *
   * elementRegistry.get('SomeElementId_1');
   * elementRegistry.get(gfx);
   *
   *
   * @param {string|SVGElement} filter for selecting the element
   *
   * @return {djs.model.Base}
   */


  ElementRegistry.prototype.get = function (filter) {
    var id;

    if (typeof filter === 'string') {
      id = filter;
    } else {
      id = filter && attr$1(filter, ELEMENT_ID);
    }

    var container = this._elements[id];
    return container && container.element;
  };
  /**
   * Return all elements that match a given filter function.
   *
   * @param {Function} fn
   *
   * @return {Array<djs.model.Base>}
   */


  ElementRegistry.prototype.filter = function (fn) {
    var filtered = [];
    this.forEach(function (element, gfx) {
      if (fn(element, gfx)) {
        filtered.push(element);
      }
    });
    return filtered;
  };
  /**
   * Return the first element that satisfies the provided testing function.
   *
   * @param {Function} fn
   *
   * @return {djs.model.Base}
   */


  ElementRegistry.prototype.find = function (fn) {
    var map = this._elements,
        keys = Object.keys(map);

    for (var i = 0; i < keys.length; i++) {
      var id = keys[i],
          container = map[id],
          element = container.element,
          gfx = container.gfx;

      if (fn(element, gfx)) {
        return element;
      }
    }
  };
  /**
   * Return all rendered model elements.
   *
   * @return {Array<djs.model.Base>}
   */


  ElementRegistry.prototype.getAll = function () {
    return this.filter(function (e) {
      return e;
    });
  };
  /**
   * Iterate over all diagram elements.
   *
   * @param {Function} fn
   */


  ElementRegistry.prototype.forEach = function (fn) {
    var map = this._elements;
    Object.keys(map).forEach(function (id) {
      var container = map[id],
          element = container.element,
          gfx = container.gfx;
      return fn(element, gfx);
    });
  };
  /**
   * Return the graphical representation of an element or its id.
   *
   * @example
   * elementRegistry.getGraphics('SomeElementId_1');
   * elementRegistry.getGraphics(rootElement); // <g ...>
   *
   * elementRegistry.getGraphics(rootElement, true); // <svg ...>
   *
   *
   * @param {string|djs.model.Base} filter
   * @param {boolean} [secondary=false] whether to return the secondary connected element
   *
   * @return {SVGElement}
   */


  ElementRegistry.prototype.getGraphics = function (filter, secondary) {
    var id = filter.id || filter;
    var container = this._elements[id];
    return container && (secondary ? container.secondaryGfx : container.gfx);
  };
  /**
   * Validate the suitability of the given id and signals a problem
   * with an exception.
   *
   * @param {string} id
   *
   * @throws {Error} if id is empty or already assigned
   */


  ElementRegistry.prototype._validateId = function (id) {
    if (!id) {
      throw new Error('element must have an id');
    }

    if (this._elements[id]) {
      throw new Error('element with id ' + id + ' already added');
    }
  };

  /**
   * An empty collection stub. Use {@link RefsCollection.extend} to extend a
   * collection with ref semantics.
   *
   * @class RefsCollection
   */

  /**
   * Extends a collection with {@link Refs} aware methods
   *
   * @memberof RefsCollection
   * @static
   *
   * @param  {Array<Object>} collection
   * @param  {Refs} refs instance
   * @param  {Object} property represented by the collection
   * @param  {Object} target object the collection is attached to
   *
   * @return {RefsCollection<Object>} the extended array
   */

  function extend$1(collection, refs, property, target) {
    var inverseProperty = property.inverse;
    /**
     * Removes the given element from the array and returns it.
     *
     * @method RefsCollection#remove
     *
     * @param {Object} element the element to remove
     */

    Object.defineProperty(collection, 'remove', {
      value: function value(element) {
        var idx = this.indexOf(element);

        if (idx !== -1) {
          this.splice(idx, 1); // unset inverse

          refs.unset(element, inverseProperty, target);
        }

        return element;
      }
    });
    /**
     * Returns true if the collection contains the given element
     *
     * @method RefsCollection#contains
     *
     * @param {Object} element the element to check for
     */

    Object.defineProperty(collection, 'contains', {
      value: function value(element) {
        return this.indexOf(element) !== -1;
      }
    });
    /**
     * Adds an element to the array, unless it exists already (set semantics).
     *
     * @method RefsCollection#add
     *
     * @param {Object} element the element to add
     * @param {Number} optional index to add element to
     *                 (possibly moving other elements around)
     */

    Object.defineProperty(collection, 'add', {
      value: function value(element, idx) {
        var currentIdx = this.indexOf(element);

        if (typeof idx === 'undefined') {
          if (currentIdx !== -1) {
            // element already in collection (!)
            return;
          } // add to end of array, as no idx is specified


          idx = this.length;
        } // handle already in collection


        if (currentIdx !== -1) {
          // remove element from currentIdx
          this.splice(currentIdx, 1);
        } // add element at idx


        this.splice(idx, 0, element);

        if (currentIdx === -1) {
          // set inverse, unless element was
          // in collection already
          refs.set(element, inverseProperty, target);
        }
      }
    }); // a simple marker, identifying this element
    // as being a refs collection

    Object.defineProperty(collection, '__refs_collection', {
      value: true
    });
    return collection;
  }

  function isExtended(collection) {
    return collection.__refs_collection === true;
  }

  var extend_1 = extend$1;
  var isExtended_1 = isExtended;
  var collection = {
    extend: extend_1,
    isExtended: isExtended_1
  };

  function hasOwnProperty$1(e, property) {
    return Object.prototype.hasOwnProperty.call(e, property.name || property);
  }

  function defineCollectionProperty(ref, property, target) {
    var collection$1 = collection.extend(target[property.name] || [], ref, property, target);
    Object.defineProperty(target, property.name, {
      enumerable: property.enumerable,
      value: collection$1
    });

    if (collection$1.length) {
      collection$1.forEach(function (o) {
        ref.set(o, property.inverse, target);
      });
    }
  }

  function defineProperty$1(ref, property, target) {
    var inverseProperty = property.inverse;
    var _value = target[property.name];
    Object.defineProperty(target, property.name, {
      configurable: property.configurable,
      enumerable: property.enumerable,
      get: function get() {
        return _value;
      },
      set: function set(value) {
        // return if we already performed all changes
        if (value === _value) {
          return;
        }

        var old = _value; // temporary set null

        _value = null;

        if (old) {
          ref.unset(old, inverseProperty, target);
        } // set new value


        _value = value; // set inverse value

        ref.set(_value, inverseProperty, target);
      }
    });
  }
  /**
   * Creates a new references object defining two inversly related
   * attribute descriptors a and b.
   *
   * <p>
   *   When bound to an object using {@link Refs#bind} the references
   *   get activated and ensure that add and remove operations are applied
   *   reversely, too.
   * </p>
   *
   * <p>
   *   For attributes represented as collections {@link Refs} provides the
   *   {@link RefsCollection#add}, {@link RefsCollection#remove} and {@link RefsCollection#contains} extensions
   *   that must be used to properly hook into the inverse change mechanism.
   * </p>
   *
   * @class Refs
   *
   * @classdesc A bi-directional reference between two attributes.
   *
   * @param {Refs.AttributeDescriptor} a property descriptor
   * @param {Refs.AttributeDescriptor} b property descriptor
   *
   * @example
   *
   * var refs = Refs({ name: 'wheels', collection: true, enumerable: true }, { name: 'car' });
   *
   * var car = { name: 'toyota' };
   * var wheels = [{ pos: 'front-left' }, { pos: 'front-right' }];
   *
   * refs.bind(car, 'wheels');
   *
   * car.wheels // []
   * car.wheels.add(wheels[0]);
   * car.wheels.add(wheels[1]);
   *
   * car.wheels // [{ pos: 'front-left' }, { pos: 'front-right' }]
   *
   * wheels[0].car // { name: 'toyota' };
   * car.wheels.remove(wheels[0]);
   *
   * wheels[0].car // undefined
   */


  function Refs(a, b) {
    if (!(this instanceof Refs)) {
      return new Refs(a, b);
    } // link


    a.inverse = b;
    b.inverse = a;
    this.props = {};
    this.props[a.name] = a;
    this.props[b.name] = b;
  }
  /**
   * Binds one side of a bi-directional reference to a
   * target object.
   *
   * @memberOf Refs
   *
   * @param  {Object} target
   * @param  {String} property
   */


  Refs.prototype.bind = function (target, property) {
    if (typeof property === 'string') {
      if (!this.props[property]) {
        throw new Error('no property <' + property + '> in ref');
      }

      property = this.props[property];
    }

    if (property.collection) {
      defineCollectionProperty(this, property, target);
    } else {
      defineProperty$1(this, property, target);
    }
  };

  Refs.prototype.ensureRefsCollection = function (target, property) {
    var collection$1 = target[property.name];

    if (!collection.isExtended(collection$1)) {
      defineCollectionProperty(this, property, target);
    }

    return collection$1;
  };

  Refs.prototype.ensureBound = function (target, property) {
    if (!hasOwnProperty$1(target, property)) {
      this.bind(target, property);
    }
  };

  Refs.prototype.unset = function (target, property, value) {
    if (target) {
      this.ensureBound(target, property);

      if (property.collection) {
        this.ensureRefsCollection(target, property).remove(value);
      } else {
        target[property.name] = undefined;
      }
    }
  };

  Refs.prototype.set = function (target, property, value) {
    if (target) {
      this.ensureBound(target, property);

      if (property.collection) {
        this.ensureRefsCollection(target, property).add(value);
      } else {
        target[property.name] = value;
      }
    }
  };

  var refs = Refs;

  var objectRefs = refs;
  var Collection = collection;
  objectRefs.Collection = Collection;

  var parentRefs = new objectRefs({
    name: 'children',
    enumerable: true,
    collection: true
  }, {
    name: 'parent'
  }),
      labelRefs = new objectRefs({
    name: 'labels',
    enumerable: true,
    collection: true
  }, {
    name: 'labelTarget'
  }),
      attacherRefs = new objectRefs({
    name: 'attachers',
    collection: true
  }, {
    name: 'host'
  }),
      outgoingRefs = new objectRefs({
    name: 'outgoing',
    collection: true
  }, {
    name: 'source'
  }),
      incomingRefs = new objectRefs({
    name: 'incoming',
    collection: true
  }, {
    name: 'target'
  });
  /**
   * @namespace djs.model
   */

  /**
   * @memberOf djs.model
   */

  /**
   * The basic graphical representation
   *
   * @class
   *
   * @abstract
   */

  function Base$1() {
    /**
     * The object that backs up the shape
     *
     * @name Base#businessObject
     * @type Object
     */
    Object.defineProperty(this, 'businessObject', {
      writable: true
    });
    /**
     * Single label support, will mapped to multi label array
     *
     * @name Base#label
     * @type Object
     */

    Object.defineProperty(this, 'label', {
      get: function get() {
        return this.labels[0];
      },
      set: function set(newLabel) {
        var label = this.label,
            labels = this.labels;

        if (!newLabel && label) {
          labels.remove(label);
        } else {
          labels.add(newLabel, 0);
        }
      }
    });
    /**
     * The parent shape
     *
     * @name Base#parent
     * @type Shape
     */

    parentRefs.bind(this, 'parent');
    /**
     * The list of labels
     *
     * @name Base#labels
     * @type Label
     */

    labelRefs.bind(this, 'labels');
    /**
     * The list of outgoing connections
     *
     * @name Base#outgoing
     * @type Array<Connection>
     */

    outgoingRefs.bind(this, 'outgoing');
    /**
     * The list of incoming connections
     *
     * @name Base#incoming
     * @type Array<Connection>
     */

    incomingRefs.bind(this, 'incoming');
  }
  /**
   * A graphical object
   *
   * @class
   * @constructor
   *
   * @extends Base
   */

  function Shape() {
    Base$1.call(this);
    /**
     * Indicates frame shapes
     *
     * @name Shape#isFrame
     * @type boolean
     */

    /**
     * The list of children
     *
     * @name Shape#children
     * @type Array<Base>
     */

    parentRefs.bind(this, 'children');
    /**
     * @name Shape#host
     * @type Shape
     */

    attacherRefs.bind(this, 'host');
    /**
     * @name Shape#attachers
     * @type Shape
     */

    attacherRefs.bind(this, 'attachers');
  }
  inherits_browser(Shape, Base$1);
  /**
   * A root graphical object
   *
   * @class
   * @constructor
   *
   * @extends Shape
   */

  function Root() {
    Shape.call(this);
  }
  inherits_browser(Root, Shape);
  /**
   * A label for an element
   *
   * @class
   * @constructor
   *
   * @extends Shape
   */

  function Label() {
    Shape.call(this);
    /**
     * The labeled element
     *
     * @name Label#labelTarget
     * @type Base
     */

    labelRefs.bind(this, 'labelTarget');
  }
  inherits_browser(Label, Shape);
  /**
   * A connection between two elements
   *
   * @class
   * @constructor
   *
   * @extends Base
   */

  function Connection() {
    Base$1.call(this);
    /**
     * The element this connection originates from
     *
     * @name Connection#source
     * @type Base
     */

    outgoingRefs.bind(this, 'source');
    /**
     * The element this connection points to
     *
     * @name Connection#target
     * @type Base
     */

    incomingRefs.bind(this, 'target');
  }
  inherits_browser(Connection, Base$1);
  var types$6 = {
    connection: Connection,
    shape: Shape,
    label: Label,
    root: Root
  };
  /**
   * Creates a new model element of the specified type
   *
   * @method create
   *
   * @example
   *
   * var shape1 = Model.create('shape', { x: 10, y: 10, width: 100, height: 100 });
   * var shape2 = Model.create('shape', { x: 210, y: 210, width: 100, height: 100 });
   *
   * var connection = Model.create('connection', { waypoints: [ { x: 110, y: 55 }, {x: 210, y: 55 } ] });
   *
   * @param  {string} type lower-cased model name
   * @param  {Object} attrs attributes to initialize the new model instance with
   *
   * @return {Base} the new model instance
   */

  function create$1(type, attrs) {
    var Type = types$6[type];

    if (!Type) {
      throw new Error('unknown type: <' + type + '>');
    }

    return assign(new Type(), attrs);
  }

  /**
   * A factory for diagram-js shapes
   */

  function ElementFactory() {
    this._uid = 12;
  }

  ElementFactory.prototype.createRoot = function (attrs) {
    return this.create('root', attrs);
  };

  ElementFactory.prototype.createLabel = function (attrs) {
    return this.create('label', attrs);
  };

  ElementFactory.prototype.createShape = function (attrs) {
    return this.create('shape', attrs);
  };

  ElementFactory.prototype.createConnection = function (attrs) {
    return this.create('connection', attrs);
  };
  /**
   * Create a model element with the given type and
   * a number of pre-set attributes.
   *
   * @param  {string} type
   * @param  {Object} attrs
   * @return {djs.model.Base} the newly created model instance
   */


  ElementFactory.prototype.create = function (type, attrs) {
    attrs = assign({}, attrs || {});

    if (!attrs.id) {
      attrs.id = type + '_' + this._uid++;
    }

    return create$1(type, attrs);
  };

  /**
   * SVGs for elements are generated by the {@link GraphicsFactory}.
   *
   * This utility gives quick access to the important semantic
   * parts of an element.
   */

  /**
   * Returns the visual part of a diagram element
   *
   * @param {Snap<SVGElement>} gfx
   *
   * @return {Snap<SVGElement>}
   */
  function getVisual(gfx) {
    return gfx.childNodes[0];
  }
  /**
   * Returns the children for a given diagram element.
   *
   * @param {Snap<SVGElement>} gfx
   * @return {Snap<SVGElement>}
   */

  function getChildren(gfx) {
    return gfx.parentNode.childNodes[1];
  }

  /**
   * @param {SVGElement} element
   * @param {number} x
   * @param {number} y
   */

  function translate(gfx, x, y) {
    var translate = createTransform();
    translate.setTranslate(x, y);
    transform(gfx, translate);
  }

  /**
   * A factory that creates graphical elements
   *
   * @param {EventBus} eventBus
   * @param {ElementRegistry} elementRegistry
   */

  function GraphicsFactory(eventBus, elementRegistry) {
    this._eventBus = eventBus;
    this._elementRegistry = elementRegistry;
  }
  GraphicsFactory.$inject = ['eventBus', 'elementRegistry'];

  GraphicsFactory.prototype._getChildrenContainer = function (element) {
    var gfx = this._elementRegistry.getGraphics(element);

    var childrenGfx; // root element

    if (!element.parent) {
      childrenGfx = gfx;
    } else {
      childrenGfx = getChildren(gfx);

      if (!childrenGfx) {
        childrenGfx = create('g');
        classes$1(childrenGfx).add('djs-children');
        append(gfx.parentNode, childrenGfx);
      }
    }

    return childrenGfx;
  };
  /**
   * Clears the graphical representation of the element and returns the
   * cleared visual (the <g class="djs-visual" /> element).
   */


  GraphicsFactory.prototype._clear = function (gfx) {
    var visual = getVisual(gfx);
    clear(visual);
    return visual;
  };
  /**
   * Creates a gfx container for shapes and connections
   *
   * The layout is as follows:
   *
   * <g class="djs-group">
   *
   *   <!-- the gfx -->
   *   <g class="djs-element djs-(shape|connection|frame)">
   *     <g class="djs-visual">
   *       <!-- the renderer draws in here -->
   *     </g>
   *
   *     <!-- extensions (overlays, click box, ...) goes here
   *   </g>
   *
   *   <!-- the gfx child nodes -->
   *   <g class="djs-children"></g>
   * </g>
   *
   * @param {string} type the type of the element, i.e. shape | connection
   * @param {SVGElement} [childrenGfx]
   * @param {number} [parentIndex] position to create container in parent
   * @param {boolean} [isFrame] is frame element
   *
   * @return {SVGElement}
   */


  GraphicsFactory.prototype._createContainer = function (type, childrenGfx, parentIndex, isFrame) {
    var outerGfx = create('g');
    classes$1(outerGfx).add('djs-group'); // insert node at position

    if (typeof parentIndex !== 'undefined') {
      prependTo(outerGfx, childrenGfx, childrenGfx.childNodes[parentIndex]);
    } else {
      append(childrenGfx, outerGfx);
    }

    var gfx = create('g');
    classes$1(gfx).add('djs-element');
    classes$1(gfx).add('djs-' + type);

    if (isFrame) {
      classes$1(gfx).add('djs-frame');
    }

    append(outerGfx, gfx); // create visual

    var visual = create('g');
    classes$1(visual).add('djs-visual');
    append(gfx, visual);
    return gfx;
  };

  GraphicsFactory.prototype.create = function (type, element, parentIndex) {
    var childrenGfx = this._getChildrenContainer(element.parent);

    return this._createContainer(type, childrenGfx, parentIndex, isFrameElement(element));
  };

  GraphicsFactory.prototype.updateContainments = function (elements) {
    var self = this,
        elementRegistry = this._elementRegistry,
        parents;
    parents = reduce(elements, function (map, e) {
      if (e.parent) {
        map[e.parent.id] = e.parent;
      }

      return map;
    }, {}); // update all parents of changed and reorganized their children
    // in the correct order (as indicated in our model)

    forEach(parents, function (parent) {
      var children = parent.children;

      if (!children) {
        return;
      }

      var childrenGfx = self._getChildrenContainer(parent);

      forEach(children.slice().reverse(), function (child) {
        var childGfx = elementRegistry.getGraphics(child);
        prependTo(childGfx.parentNode, childrenGfx);
      });
    });
  };

  GraphicsFactory.prototype.drawShape = function (visual, element) {
    var eventBus = this._eventBus;
    return eventBus.fire('render.shape', {
      gfx: visual,
      element: element
    });
  };

  GraphicsFactory.prototype.getShapePath = function (element) {
    var eventBus = this._eventBus;
    return eventBus.fire('render.getShapePath', element);
  };

  GraphicsFactory.prototype.drawConnection = function (visual, element) {
    var eventBus = this._eventBus;
    return eventBus.fire('render.connection', {
      gfx: visual,
      element: element
    });
  };

  GraphicsFactory.prototype.getConnectionPath = function (waypoints) {
    var eventBus = this._eventBus;
    return eventBus.fire('render.getConnectionPath', waypoints);
  };

  GraphicsFactory.prototype.update = function (type, element, gfx) {
    // do NOT update root element
    if (!element.parent) {
      return;
    }

    var visual = this._clear(gfx); // redraw


    if (type === 'shape') {
      this.drawShape(visual, element); // update positioning

      translate(gfx, element.x, element.y);
    } else if (type === 'connection') {
      this.drawConnection(visual, element);
    } else {
      throw new Error('unknown type: ' + type);
    }

    if (element.hidden) {
      attr$1(gfx, 'display', 'none');
    } else {
      attr$1(gfx, 'display', 'block');
    }
  };

  GraphicsFactory.prototype.remove = function (element) {
    var gfx = this._elementRegistry.getGraphics(element); // remove


    remove$1(gfx.parentNode);
  }; // helpers //////////


  function prependTo(newNode, parentNode, siblingNode) {
    var node = siblingNode || parentNode.firstChild; // do not prepend node to itself to prevent IE from crashing
    // https://github.com/bpmn-io/bpmn-js/issues/746

    if (newNode === node) {
      return;
    }

    parentNode.insertBefore(newNode, node);
  }

  var CoreModule = {
    __depends__: [DrawModule],
    __init__: ['canvas'],
    canvas: ['type', Canvas],
    elementRegistry: ['type', ElementRegistry],
    elementFactory: ['type', ElementFactory],
    eventBus: ['type', EventBus],
    graphicsFactory: ['type', GraphicsFactory]
  };

  /**
   * Bootstrap an injector from a list of modules, instantiating a number of default components
   *
   * @ignore
   * @param {Array<didi.Module>} bootstrapModules
   *
   * @return {didi.Injector} a injector to use to access the components
   */

  function bootstrap(bootstrapModules) {
    var modules = [],
        components = [];

    function hasModule(m) {
      return modules.indexOf(m) >= 0;
    }

    function addModule(m) {
      modules.push(m);
    }

    function visit(m) {
      if (hasModule(m)) {
        return;
      }

      (m.__depends__ || []).forEach(visit);

      if (hasModule(m)) {
        return;
      }

      addModule(m);
      (m.__init__ || []).forEach(function (c) {
        components.push(c);
      });
    }

    bootstrapModules.forEach(visit);
    var injector = new Injector(modules);
    components.forEach(function (c) {
      try {
        // eagerly resolve component (fn or string)
        injector[typeof c === 'string' ? 'get' : 'invoke'](c);
      } catch (e) {
        console.error('Failed to instantiate component');
        console.error(e.stack);
        throw e;
      }
    });
    return injector;
  }
  /**
   * Creates an injector from passed options.
   *
   * @ignore
   * @param  {Object} options
   * @return {didi.Injector}
   */


  function createInjector(options) {
    options = options || {};
    var configModule = {
      'config': ['value', options]
    };
    var modules = [configModule, CoreModule].concat(options.modules || []);
    return bootstrap(modules);
  }
  /**
   * The main diagram-js entry point that bootstraps the diagram with the given
   * configuration.
   *
   * To register extensions with the diagram, pass them as Array<didi.Module> to the constructor.
   *
   * @class djs.Diagram
   * @memberOf djs
   * @constructor
   *
   * @example
   *
   * <caption>Creating a plug-in that logs whenever a shape is added to the canvas.</caption>
   *
   * // plug-in implemenentation
   * function MyLoggingPlugin(eventBus) {
   *   eventBus.on('shape.added', function(event) {
   *     console.log('shape ', event.shape, ' was added to the diagram');
   *   });
   * }
   *
   * // export as module
   * export default {
   *   __init__: [ 'myLoggingPlugin' ],
   *     myLoggingPlugin: [ 'type', MyLoggingPlugin ]
   * };
   *
   *
   * // instantiate the diagram with the new plug-in
   *
   * import MyLoggingModule from 'path-to-my-logging-plugin';
   *
   * var diagram = new Diagram({
   *   modules: [
   *     MyLoggingModule
   *   ]
   * });
   *
   * diagram.invoke([ 'canvas', function(canvas) {
   *   // add shape to drawing canvas
   *   canvas.addShape({ x: 10, y: 10 });
   * });
   *
   * // 'shape ... was added to the diagram' logged to console
   *
   * @param {Object} options
   * @param {Array<didi.Module>} [options.modules] external modules to instantiate with the diagram
   * @param {didi.Injector} [injector] an (optional) injector to bootstrap the diagram with
   */


  function Diagram(options, injector) {
    // create injector unless explicitly specified
    this.injector = injector = injector || createInjector(options); // API

    /**
     * Resolves a diagram service
     *
     * @method Diagram#get
     *
     * @param {string} name the name of the diagram service to be retrieved
     * @param {boolean} [strict=true] if false, resolve missing services to null
     */

    this.get = injector.get;
    /**
     * Executes a function into which diagram services are injected
     *
     * @method Diagram#invoke
     *
     * @param {Function|Object[]} fn the function to resolve
     * @param {Object} locals a number of locals to use to resolve certain dependencies
     */

    this.invoke = injector.invoke; // init
    // indicate via event

    /**
     * An event indicating that all plug-ins are loaded.
     *
     * Use this event to fire other events to interested plug-ins
     *
     * @memberOf Diagram
     *
     * @event diagram.init
     *
     * @example
     *
     * eventBus.on('diagram.init', function() {
     *   eventBus.fire('my-custom-event', { foo: 'BAR' });
     * });
     *
     * @type {Object}
     */

    this.get('eventBus').fire('diagram.init');
  }
  /**
   * Destroys the diagram
   *
   * @method  Diagram#destroy
   */

  Diagram.prototype.destroy = function () {
    this.get('eventBus').fire('diagram.destroy');
  };
  /**
   * Clear the diagram, removing all contents.
   */


  Diagram.prototype.clear = function () {
    this.get('eventBus').fire('diagram.clear');
  };

  var inherits_browser$1 = createCommonjsModule(function (module) {
    if (typeof Object.create === 'function') {
      // implementation from standard node.js 'util' module
      module.exports = function inherits(ctor, superCtor) {
        ctor.super_ = superCtor;
        ctor.prototype = Object.create(superCtor.prototype, {
          constructor: {
            value: ctor,
            enumerable: false,
            writable: true,
            configurable: true
          }
        });
      };
    } else {
      // old school shim for old browsers
      module.exports = function inherits(ctor, superCtor) {
        ctor.super_ = superCtor;

        var TempCtor = function TempCtor() {};

        TempCtor.prototype = superCtor.prototype;
        ctor.prototype = new TempCtor();
        ctor.prototype.constructor = ctor;
      };
    }
  });

  /**
   * Is an element of the given DMN type?
   *
   * @param  {tjs.model.Base|ModdleElement} element
   * @param  {string} type
   *
   * @return {boolean}
   */

  function is(element, type) {
    var bo = getBusinessObject(element);
    return bo && typeof bo.$instanceOf === 'function' && bo.$instanceOf(type);
  }
  /**
   * Return the business object for a given element.
   *
   * @param  {tjs.model.Base|ModdleElement} element
   *
   * @return {ModdleElement}
   */

  function getBusinessObject(element) {
    return element && element.businessObject || element;
  }
  function getName(element) {
    return getBusinessObject(element).name;
  }

  var diRefs = new objectRefs({
    name: 'dmnElementRef',
    enumerable: true
  }, {
    name: 'di',
    configurable: true
  });
  function DRDTreeWalker(handler, options) {
    // list of elements to handle deferred to ensure
    // prerequisites are drawn
    var deferred = [];

    function visit(element) {
      var gfx = element.gfx; // avoid multiple rendering of elements

      if (gfx) {
        throw new Error('already rendered ' + element.id);
      } // call handler


      return handler.element(element);
    }

    function visitRoot(element) {
      return handler.root(element);
    }

    function visitIfDi(element) {
      try {
        var gfx = element.di && visit(element);
        return gfx;
      } catch (e) {
        logError(e.message, {
          element: element,
          error: e
        });
      }
    } // Semantic handling //////////////////////

    /**
     * Handle definitions and return the rendered diagram (if any)
     *
     * @param {ModdleElement} definitions to walk and import
     * @param {ModdleElement} [diagram] specific diagram to import and display
     *
     * @throws {Error} if no diagram to display could be found
     */


    function handleDefinitions(definitions, diagram) {
      // make sure we walk the correct dmnElement
      var dmnDI = definitions.dmnDI;

      if (!dmnDI) {
        throw new Error('no dmndi:DMNDI');
      }

      var diagrams = dmnDI.diagrams || [];

      if (diagram && diagrams.indexOf(diagram) === -1) {
        throw new Error('diagram not part of dmndi:DMNDI');
      }

      if (!diagram && diagrams && diagrams.length) {
        diagram = diagrams[0];
      } // no diagram -> nothing to import


      if (!diagram) {
        throw new Error('no diagram to display');
      } // assign current diagram to definitions so that it can accessed later


      definitions.di = diagram; // load DI from selected diagram only

      handleDiagram(diagram);
      visitRoot(definitions);
      handleDrgElements(definitions.get('drgElement'));
      handleArtifacts(definitions.get('artifact'));
      handleDeferred();
    }

    function handleDrgElements(elements) {
      forEach(elements, function (element) {
        visitIfDi(element);
        handleRequirements(element);
      });
    }

    function handleArtifacts(elements) {
      forEach(elements, function (element) {
        if (is(element, 'dmn:Association')) {
          handleAssociation(element);
        } else {
          visitIfDi(element);
        }
      });
    }
    /**
     * Defer association visit until all shapes are visited.
     *
     * @param {ModdleElement} element
     */


    function handleAssociation(element) {
      defer(function () {
        visitIfDi(element);
      });
    }
    /**
     * Defer requirements visiting until all shapes are visited.
     *
     * @param {ModdleElement} element
     */


    function handleRequirements(element) {
      forEach(['informationRequirement', 'knowledgeRequirement', 'authorityRequirement'], function (requirements) {
        forEach(element[requirements], function (requirement) {
          defer(function () {
            visitIfDi(requirement);
          });
        });
      });
    } // DI handling //////////////////////


    function handleDiagram(diagram) {
      forEach(diagram.diagramElements, handleDiagramElement);
    }

    function handleDiagramElement(diagramElement) {
      registerDi(diagramElement);
    }

    function registerDi(di) {
      var dmnElement = di.dmnElementRef;

      if (dmnElement) {
        if (dmnElement.di) {
          logError('multiple DI elements defined for element', {
            element: dmnElement
          });
        } else {
          diRefs.bind(dmnElement, 'di');
          dmnElement.di = di;
        }
      } else {
        logError('no DMN element referenced in element', {
          element: di
        });
      }
    }

    function defer(fn) {
      deferred.push(fn);
    }

    function handleDeferred() {
      forEach(deferred, function (d) {
        d();
      });
    }

    function logError(message, context) {
      handler.error(message, context);
    } // API //////////////////////


    return {
      handleDefinitions: handleDefinitions
    };
  }

  /**
   * Import the definitions into a diagram.
   *
   * Errors and warnings are reported through the specified callback.
   *
   * @param  {Drd} drd
   * @param  {ModdleElement} definitions
   * @param  {Function} done
   *         the callback, invoked with (err, [ warning ]) once the import is done
   */

  function importDRD(drd, definitions, done) {
    var importer = drd.get('drdImporter'),
        eventBus = drd.get('eventBus');
    var error,
        warnings = [];

    function render(definitions) {
      var visitor = {
        root: function root(element) {
          return importer.root(element);
        },
        element: function element(_element, di) {
          return importer.add(_element, di);
        },
        error: function error(message, context) {
          warnings.push({
            message: message,
            context: context
          });
        }
      };
      var walker = new DRDTreeWalker(visitor); // import

      walker.handleDefinitions(definitions);
    }

    eventBus.fire('import.start', {
      definitions: definitions
    });

    try {
      render(definitions);
    } catch (e) {
      error = e;
    }

    eventBus.fire('import.done', {
      error: error,
      warnings: warnings
    });
    done(error, warnings);
  }

  function DrdRenderer(eventBus, pathMap, styles, textRenderer) {
    BaseRenderer.call(this, eventBus);
    var markers = {};

    function addMarker(id, element) {
      markers[id] = element;
    }

    function marker(id) {
      var marker = markers[id];
      return 'url(#' + marker.id + ')';
    }

    function initMarkers(svg) {
      function createMarker(id, options) {
        var attrs = assign({
          strokeWidth: 1,
          strokeLinecap: 'round',
          strokeDasharray: 'none'
        }, options.attrs);
        var ref = options.ref || {
          x: 0,
          y: 0
        };
        var scale = options.scale || 1; // fix for safari / chrome / firefox bug not correctly
        // resetting stroke dash array

        if (attrs.strokeDasharray === 'none') {
          attrs.strokeDasharray = [10000, 1];
        }

        var marker = create('marker');
        attr$1(options.element, attrs);
        append(marker, options.element);
        attr$1(marker, {
          id: id,
          viewBox: '0 0 20 20',
          refX: ref.x,
          refY: ref.y,
          markerWidth: 20 * scale,
          markerHeight: 20 * scale,
          orient: 'auto'
        });
        var defs = query('defs', svg);

        if (!defs) {
          defs = create('defs');
          append(svg, defs);
        }

        append(defs, marker);
        return addMarker(id, marker);
      }

      var associationStart = create('path');
      attr$1(associationStart, {
        d: 'M 11 5 L 1 10 L 11 15'
      });
      createMarker('association-start', {
        element: associationStart,
        attrs: {
          fill: 'none',
          stroke: 'black',
          strokeWidth: 1.5
        },
        ref: {
          x: 1,
          y: 10
        },
        scale: 0.5
      });
      var associationEnd = create('path');
      attr$1(associationEnd, {
        d: 'M 1 5 L 11 10 L 1 15'
      });
      createMarker('association-end', {
        element: associationEnd,
        attrs: {
          fill: 'none',
          stroke: 'black',
          strokeWidth: 1.5
        },
        ref: {
          x: 12,
          y: 10
        },
        scale: 0.5
      });
      var informationRequirementEnd = create('path');
      attr$1(informationRequirementEnd, {
        d: 'M 1 5 L 11 10 L 1 15 Z'
      });
      createMarker('information-requirement-end', {
        element: informationRequirementEnd,
        ref: {
          x: 11,
          y: 10
        },
        scale: 1
      });
      var knowledgeRequirementEnd = create('path');
      attr$1(knowledgeRequirementEnd, {
        d: 'M 1 3 L 11 10 L 1 17'
      });
      createMarker('knowledge-requirement-end', {
        element: knowledgeRequirementEnd,
        attrs: {
          fill: 'none',
          stroke: 'black',
          strokeWidth: 2
        },
        ref: {
          x: 11,
          y: 10
        },
        scale: 0.8
      });
      var authorityRequirementEnd = create('circle');
      attr$1(authorityRequirementEnd, {
        cx: 3,
        cy: 3,
        r: 3
      });
      createMarker('authority-requirement-end', {
        element: authorityRequirementEnd,
        ref: {
          x: 3,
          y: 3
        },
        scale: 0.9
      });
    }

    function computeStyle(custom, traits, defaultStyles) {
      if (!isArray(traits)) {
        defaultStyles = traits;
        traits = [];
      }

      return styles.style(traits || [], assign(defaultStyles, custom || {}));
    }

    function drawRect(p, width, height, r, offset, attrs) {
      if (isObject(offset)) {
        attrs = offset;
        offset = 0;
      }

      offset = offset || 0;
      attrs = computeStyle(attrs, {
        stroke: 'black',
        strokeWidth: 2,
        fill: 'white'
      });
      var rect = create('rect');
      attr$1(rect, {
        x: offset,
        y: offset,
        width: width - offset * 2,
        height: height - offset * 2,
        rx: r,
        ry: r
      });
      attr$1(rect, attrs);
      append(p, rect);
      return rect;
    }

    function renderLabel(p, label, options) {
      var text = textRenderer.createText(label || '', options);
      attr(text, 'class', 'djs-label');
      append(p, text);
      return text;
    }

    function renderEmbeddedLabel(p, element, align) {
      var name = getName(element);
      return renderLabel(p, name, {
        box: element,
        align: align,
        padding: 5
      });
    }

    function drawPath(p, d, attrs) {
      attrs = computeStyle(attrs, ['no-fill'], {
        strokeWidth: 2,
        stroke: 'black'
      });
      var path = create('path');
      attr$1(path, {
        d: d
      });
      attr$1(path, attrs);
      append(p, path);
      return path;
    }

    var handlers = {
      'dmn:Decision': function dmnDecision(p, element, attrs) {
        var rect = drawRect(p, element.width, element.height, 0, attrs);
        renderEmbeddedLabel(p, element, 'center-middle');
        return rect;
      },
      'dmn:KnowledgeSource': function dmnKnowledgeSource(p, element, attrs) {
        var pathData = pathMap.getScaledPath('KNOWLEDGE_SOURCE', {
          xScaleFactor: 1.021,
          yScaleFactor: 1,
          containerWidth: element.width,
          containerHeight: element.height,
          position: {
            mx: 0.0,
            my: 0.075
          }
        });
        var knowledgeSource = drawPath(p, pathData, {
          strokeWidth: 2,
          fill: 'white',
          stroke: 'black'
        });
        renderEmbeddedLabel(p, element, 'center-middle');
        return knowledgeSource;
      },
      'dmn:BusinessKnowledgeModel': function dmnBusinessKnowledgeModel(p, element, attrs) {
        var pathData = pathMap.getScaledPath('BUSINESS_KNOWLEDGE_MODEL', {
          xScaleFactor: 1,
          yScaleFactor: 1,
          containerWidth: element.width,
          containerHeight: element.height,
          position: {
            mx: 0.0,
            my: 0.3
          }
        });
        var businessKnowledge = drawPath(p, pathData, {
          strokeWidth: 2,
          fill: 'white',
          stroke: 'black'
        });
        renderEmbeddedLabel(p, element, 'center-middle');
        return businessKnowledge;
      },
      'dmn:InputData': function dmnInputData(p, element, attrs) {
        var rect = drawRect(p, element.width, element.height, 22, attrs);
        renderEmbeddedLabel(p, element, 'center-middle');
        return rect;
      },
      'dmn:TextAnnotation': function dmnTextAnnotation(p, element, attrs) {
        var style = {
          'fill': 'none',
          'stroke': 'none'
        };
        var textElement = drawRect(p, element.width, element.height, 0, 0, style);
        var textPathData = pathMap.getScaledPath('TEXT_ANNOTATION', {
          xScaleFactor: 1,
          yScaleFactor: 1,
          containerWidth: element.width,
          containerHeight: element.height,
          position: {
            mx: 0.0,
            my: 0.0
          }
        });
        drawPath(p, textPathData);
        var text = getSemantic(element).text || '';
        renderLabel(p, text, {
          box: element,
          align: 'left-top',
          padding: 5
        });
        return textElement;
      },
      'dmn:Association': function dmnAssociation(p, element, attrs) {
        var semantic = getSemantic(element);
        attrs = assign({
          strokeDasharray: '0.5, 5',
          strokeLinecap: 'round',
          strokeLinejoin: 'round',
          fill: 'none'
        }, attrs || {});

        if (semantic.associationDirection === 'One' || semantic.associationDirection === 'Both') {
          attrs.markerEnd = marker('association-end');
        }

        if (semantic.associationDirection === 'Both') {
          attrs.markerStart = marker('association-start');
        }

        return drawLine(p, element.waypoints, attrs);
      },
      'dmn:InformationRequirement': function dmnInformationRequirement(p, element, attrs) {
        attrs = assign({
          strokeWidth: 1,
          strokeLinecap: 'round',
          strokeLinejoin: 'round',
          markerEnd: marker('information-requirement-end')
        }, attrs || {});
        return drawLine(p, element.waypoints, attrs);
      },
      'dmn:KnowledgeRequirement': function dmnKnowledgeRequirement(p, element, attrs) {
        attrs = assign({
          strokeWidth: 1,
          strokeDasharray: 5,
          strokeLinecap: 'round',
          strokeLinejoin: 'round',
          markerEnd: marker('knowledge-requirement-end')
        }, attrs || {});
        return drawLine(p, element.waypoints, attrs);
      },
      'dmn:AuthorityRequirement': function dmnAuthorityRequirement(p, element, attrs) {
        attrs = assign({
          strokeWidth: 1.5,
          strokeDasharray: 5,
          strokeLinecap: 'round',
          strokeLinejoin: 'round',
          markerEnd: marker('authority-requirement-end')
        }, attrs || {});
        return drawLine(p, element.waypoints, attrs);
      }
    }; // draw shape and connection //////////////////

    function drawShape(parent, element) {
      var h = handlers[element.type];

      if (!h) {
        return BaseRenderer.prototype.drawShape.apply(this, [parent, element]);
      } else {
        return h(parent, element);
      }
    }

    function drawConnection(parent, element) {
      var type = element.type;
      var h = handlers[type];

      if (!h) {
        return BaseRenderer.prototype.drawConnection.apply(this, [parent, element]);
      } else {
        return h(parent, element);
      }
    }

    function drawLine(p, waypoints, attrs) {
      attrs = computeStyle(attrs, ['no-fill'], {
        stroke: 'black',
        strokeWidth: 2,
        fill: 'none'
      });
      var line = createLine(waypoints, attrs);
      append(p, line);
      return line;
    }

    this.canRender = function (element) {
      return is(element, 'dmn:DMNElement') || is(element, 'dmn:InformationRequirement') || is(element, 'dmn:KnowledgeRequirement') || is(element, 'dmn:AuthorityRequirement');
    };

    this.drawShape = drawShape;
    this.drawConnection = drawConnection; // hook onto canvas init event to initialize
    // connection start/end markers on svg

    eventBus.on('canvas.init', function (event) {
      initMarkers(event.svg);
    });
  }
  inherits_browser$1(DrdRenderer, BaseRenderer);
  DrdRenderer.$inject = ['eventBus', 'pathMap', 'styles', 'textRenderer']; // helper functions //////////////////////

  function getSemantic(element) {
    return element.businessObject;
  }

  var DEFAULT_BOX_PADDING = 0;
  var DEFAULT_LABEL_SIZE = {
    width: 150,
    height: 50
  };

  function parseAlign(align) {
    var parts = align.split('-');
    return {
      horizontal: parts[0] || 'center',
      vertical: parts[1] || 'top'
    };
  }

  function parsePadding(padding) {
    if (isObject(padding)) {
      return assign({
        top: 0,
        left: 0,
        right: 0,
        bottom: 0
      }, padding);
    } else {
      return {
        top: padding,
        left: padding,
        right: padding,
        bottom: padding
      };
    }
  }

  function getTextBBox(text, fakeText) {
    fakeText.textContent = text;
    var textBBox;

    try {
      var bbox,
          emptyLine = text === ''; // add dummy text, when line is empty to
      // determine correct height

      fakeText.textContent = emptyLine ? 'dummy' : text;
      textBBox = fakeText.getBBox(); // take text rendering related horizontal
      // padding into account

      bbox = {
        width: textBBox.width + textBBox.x * 2,
        height: textBBox.height
      };

      if (emptyLine) {
        // correct width
        bbox.width = 0;
      }

      return bbox;
    } catch (e) {
      return {
        width: 0,
        height: 0
      };
    }
  }
  /**
   * Layout the next line and return the layouted element.
   *
   * Alters the lines passed.
   *
   * @param  {Array<string>} lines
   * @return {Object} the line descriptor, an object { width, height, text }
   */


  function layoutNext(lines, maxWidth, fakeText) {
    var originalLine = lines.shift(),
        fitLine = originalLine;
    var textBBox;

    for (;;) {
      textBBox = getTextBBox(fitLine, fakeText);
      textBBox.width = fitLine ? textBBox.width : 0; // try to fit

      if (fitLine === ' ' || fitLine === '' || textBBox.width < Math.round(maxWidth) || fitLine.length < 2) {
        return fit(lines, fitLine, originalLine, textBBox);
      }

      fitLine = shortenLine(fitLine, textBBox.width, maxWidth);
    }
  }

  function fit(lines, fitLine, originalLine, textBBox) {
    if (fitLine.length < originalLine.length) {
      var remainder = originalLine.slice(fitLine.length).trim();
      lines.unshift(remainder);
    }

    return {
      width: textBBox.width,
      height: textBBox.height,
      text: fitLine
    };
  }

  var SOFT_BREAK = "\xAD";
  /**
   * Shortens a line based on spacing and hyphens.
   * Returns the shortened result on success.
   *
   * @param  {string} line
   * @param  {number} maxLength the maximum characters of the string
   * @return {string} the shortened string
   */

  function semanticShorten(line, maxLength) {
    var parts = line.split(/(\s|-|\u00AD)/g),
        part,
        shortenedParts = [],
        length = 0; // try to shorten via break chars

    if (parts.length > 1) {
      while (part = parts.shift()) {
        if (part.length + length < maxLength) {
          shortenedParts.push(part);
          length += part.length;
        } else {
          // remove previous part, too if hyphen does not fit anymore
          if (part === '-' || part === SOFT_BREAK) {
            shortenedParts.pop();
          }

          break;
        }
      }
    }

    var last = shortenedParts[shortenedParts.length - 1]; // translate trailing soft break to actual hyphen

    if (last && last === SOFT_BREAK) {
      shortenedParts[shortenedParts.length - 1] = '-';
    }

    return shortenedParts.join('');
  }

  function shortenLine(line, width, maxWidth) {
    var length = Math.max(line.length * (maxWidth / width), 1); // try to shorten semantically (i.e. based on spaces and hyphens)

    var shortenedLine = semanticShorten(line, length);

    if (!shortenedLine) {
      // force shorten by cutting the long word
      shortenedLine = line.slice(0, Math.max(Math.round(length - 1), 1));
    }

    return shortenedLine;
  }

  function getHelperSvg() {
    var helperSvg = document.getElementById('helper-svg');

    if (!helperSvg) {
      helperSvg = create('svg');
      attr$1(helperSvg, {
        id: 'helper-svg',
        width: 0,
        height: 0,
        style: 'visibility: hidden; position: fixed'
      });
      document.body.appendChild(helperSvg);
    }

    return helperSvg;
  }
  /**
   * Creates a new label utility
   *
   * @param {Object} config
   * @param {Dimensions} config.size
   * @param {number} config.padding
   * @param {Object} config.style
   * @param {string} config.align
   */


  function Text(config) {
    this._config = assign({}, {
      size: DEFAULT_LABEL_SIZE,
      padding: DEFAULT_BOX_PADDING,
      style: {},
      align: 'center-top'
    }, config || {});
  }
  /**
   * Returns the layouted text as an SVG element.
   *
   * @param {string} text
   * @param {Object} options
   *
   * @return {SVGElement}
   */

  Text.prototype.createText = function (text, options) {
    return this.layoutText(text, options).element;
  };
  /**
   * Returns a labels layouted dimensions.
   *
   * @param {string} text to layout
   * @param {Object} options
   *
   * @return {Dimensions}
   */


  Text.prototype.getDimensions = function (text, options) {
    return this.layoutText(text, options).dimensions;
  };
  /**
   * Creates and returns a label and its bounding box.
   *
   * @method Text#createText
   *
   * @param {string} text the text to render on the label
   * @param {Object} options
   * @param {string} options.align how to align in the bounding box.
   *                               Any of { 'center-middle', 'center-top' },
   *                               defaults to 'center-top'.
   * @param {string} options.style style to be applied to the text
   * @param {boolean} options.fitBox indicates if box will be recalculated to
   *                                 fit text
   *
   * @return {Object} { element, dimensions }
   */


  Text.prototype.layoutText = function (text, options) {
    var box = assign({}, this._config.size, options.box),
        style = assign({}, this._config.style, options.style),
        align = parseAlign(options.align || this._config.align),
        padding = parsePadding(options.padding !== undefined ? options.padding : this._config.padding),
        fitBox = options.fitBox || false;
    var lineHeight = getLineHeight(style); // we split text by lines and normalize
    // {soft break} + {line break} => { line break }

    var lines = text.split(/\u00AD?\r?\n/),
        layouted = [];
    var maxWidth = box.width - padding.left - padding.right; // ensure correct rendering by attaching helper text node to invisible SVG

    var helperText = create('text');
    attr$1(helperText, {
      x: 0,
      y: 0
    });
    attr$1(helperText, style);
    var helperSvg = getHelperSvg();
    append(helperSvg, helperText);

    while (lines.length) {
      layouted.push(layoutNext(lines, maxWidth, helperText));
    }

    if (align.vertical === 'middle') {
      padding.top = padding.bottom = 0;
    }

    var totalHeight = reduce(layouted, function (sum, line, idx) {
      return sum + (lineHeight || line.height);
    }, 0) + padding.top + padding.bottom;
    var maxLineWidth = reduce(layouted, function (sum, line, idx) {
      return line.width > sum ? line.width : sum;
    }, 0); // the y position of the next line

    var y = padding.top;

    if (align.vertical === 'middle') {
      y += (box.height - totalHeight) / 2;
    } // magic number initial offset


    y -= (lineHeight || layouted[0].height) / 4;
    var textElement = create('text');
    attr$1(textElement, style); // layout each line taking into account that parent
    // shape might resize to fit text size

    forEach(layouted, function (line) {
      var x;
      y += lineHeight || line.height;

      switch (align.horizontal) {
        case 'left':
          x = padding.left;
          break;

        case 'right':
          x = (fitBox ? maxLineWidth : maxWidth) - padding.right - line.width;
          break;

        default:
          // aka center
          x = Math.max(((fitBox ? maxLineWidth : maxWidth) - line.width) / 2 + padding.left, 0);
      }

      var tspan = create('tspan');
      attr$1(tspan, {
        x: x,
        y: y
      });
      tspan.textContent = line.text;
      append(textElement, tspan);
    });
    remove$1(helperText);
    var dimensions = {
      width: maxLineWidth,
      height: totalHeight
    };
    return {
      dimensions: dimensions,
      element: textElement
    };
  };

  function getLineHeight(style) {
    if ('fontSize' in style && 'lineHeight' in style) {
      return style.lineHeight * parseInt(style.fontSize, 10);
    }
  }

  var DEFAULT_FONT_SIZE = 12;
  var LINE_HEIGHT_RATIO = 1.2;
  var MIN_TEXT_ANNOTATION_HEIGHT = 30;
  function TextRenderer(config) {
    var defaultStyle = assign({
      fontFamily: 'Arial, sans-serif',
      fontSize: DEFAULT_FONT_SIZE,
      fontWeight: 'normal',
      lineHeight: LINE_HEIGHT_RATIO
    }, config && config.defaultStyle || {});
    var fontSize = parseInt(defaultStyle.fontSize, 10) - 1;
    var externalStyle = assign({}, defaultStyle, {
      fontSize: fontSize
    }, config && config.externalStyle || {});
    var textUtil = new Text({
      style: defaultStyle
    });
    /**
     * Get the new bounds of an externally rendered,
     * layouted label.
     *
     * @param  {Bounds} bounds
     * @param  {string} text
     *
     * @return {Bounds}
     */

    this.getExternalLabelBounds = function (bounds, text) {
      var layoutedDimensions = textUtil.getDimensions(text, {
        box: {
          width: 90,
          height: 30,
          x: bounds.width / 2 + bounds.x,
          y: bounds.height / 2 + bounds.y
        },
        style: externalStyle
      }); // resize label shape to fit label text

      return {
        x: Math.round(bounds.x + bounds.width / 2 - layoutedDimensions.width / 2),
        y: Math.round(bounds.y),
        width: Math.ceil(layoutedDimensions.width),
        height: Math.ceil(layoutedDimensions.height)
      };
    };
    /**
     * Get the new bounds of text annotation.
     *
     * @param  {Bounds} bounds
     * @param  {string} text
     *
     * @return {Bounds}
     */


    this.getTextAnnotationBounds = function (bounds, text) {
      var layoutedDimensions = textUtil.getDimensions(text, {
        box: bounds,
        style: defaultStyle,
        align: 'left-top',
        padding: 5
      });
      return {
        x: bounds.x,
        y: bounds.y,
        width: bounds.width,
        height: Math.max(MIN_TEXT_ANNOTATION_HEIGHT, Math.round(layoutedDimensions.height))
      };
    };
    /**
     * Create a layouted text element.
     *
     * @param {string} text
     * @param {Object} [options]
     *
     * @return {SVGElement} rendered text
     */


    this.createText = function (text, options) {
      return textUtil.createText(text, options || {});
    };
    /**
     * Get default text style.
     */


    this.getDefaultStyle = function () {
      return defaultStyle;
    };
    /**
     * Get the external text style.
     */


    this.getExternalStyle = function () {
      return externalStyle;
    };
  }
  TextRenderer.$inject = ['config.textRenderer'];

  /* eslint-disable max-len */

  /**
   * Map containing SVG paths needed by BpmnRenderer.
   */
  function PathMap() {
    /**
     * Contains a map of path elements
     *
     * <h1>Path definition</h1>
     * A parameterized path is defined like this:
     * <pre>
     * 'GATEWAY_PARALLEL': {
     *   d: 'm {mx},{my} {e.x0},0 0,{e.x1} {e.x1},0 0,{e.y0} -{e.x1},0 0,{e.y1} ' +
            '-{e.x0},0 0,-{e.y1} -{e.x1},0 0,-{e.y0} {e.x1},0 z',
     *   height: 17.5,
     *   width:  17.5,
     *   heightElements: [2.5, 7.5],
     *   widthElements: [2.5, 7.5]
     * }
     * </pre>
     * <p>It's important to specify a correct <b>height and width</b> for the path as the scaling
     * is based on the ratio between the specified height and width in this object and the
     * height and width that is set as scale target (Note x,y coordinates will be scaled with
     * individual ratios).</p>
     * <p>The '<b>heightElements</b>' and '<b>widthElements</b>' array must contain the values that will be scaled.
     * The scaling is based on the computed ratios.
     * Coordinates on the y axis should be in the <b>heightElement</b>'s array, they will be scaled using
     * the computed ratio coefficient.
     * In the parameterized path the scaled values can be accessed through the 'e' object in {} brackets.
     *   <ul>
     *    <li>The values for the y axis can be accessed in the path string using {e.y0}, {e.y1}, ....</li>
     *    <li>The values for the x axis can be accessed in the path string using {e.x0}, {e.x1}, ....</li>
     *   </ul>
     *   The numbers x0, x1 respectively y0, y1, ... map to the corresponding array index.
     * </p>
      m1,1
      l 0,55.3
      c 29.8,19.7 48.4,-4.2 67.2,-6.7
      c 12.2,-2.3 19.8,1.6 30.8,6.2
      l 0,-54.6
      z
      */
    this.pathMap = {
      'KNOWLEDGE_SOURCE': {
        d: 'm {mx},{my} ' + 'l 0,{e.y0} ' + 'c {e.x0},{e.y1} {e.x1},-{e.y2} {e.x2},-{e.y3} ' + 'c {e.x3},-{e.y4} {e.x4},{e.y5} {e.x5},{e.y6} ' + 'l 0,-{e.y7}z',
        width: 100,
        height: 65,
        widthElements: [29.8, 48.4, 67.2, 12.2, 19.8, 30.8],
        heightElements: [55.3, 19.7, 4.2, 6.7, 2.3, 1.6, 6.2, 54.6]
      },
      'BUSINESS_KNOWLEDGE_MODEL': {
        d: 'm {mx},{my} l {e.x0},-{e.y0} l {e.x1},0 l 0,{e.y1} l -{e.x2},{e.y2} l -{e.x3},0z',
        width: 125,
        height: 45,
        widthElements: [13.8, 109.2, 13.8, 109.1],
        heightElements: [13.2, 29.8, 13.2]
      },
      'TEXT_ANNOTATION': {
        d: 'm {mx}, {my} m 10,0 l -10,0 l 0,{e.y0} l 10,0',
        width: 10,
        height: 30,
        widthElements: [10],
        heightElements: [30]
      }
    };

    this.getRawPath = function getRawPath(pathId) {
      return this.pathMap[pathId].d;
    };
    /**
     * Scales the path to the given height and width.
     * <h1>Use case</h1>
     * <p>Use case is to scale the content of elements (event, gateways) based
     * on the element bounding box's size.
     * </p>
     * <h1>Why not transform</h1>
     * <p>Scaling a path with transform() will also scale the stroke and IE does not support
     * the option 'non-scaling-stroke' to prevent this.
     * Also there are use cases where only some parts of a path should be
     * scaled.</p>
     *
     * @param {string} pathId The ID of the path.
     * @param {Object} param <p>
     *   Example param object scales the path to 60% size of the container (data.width, data.height).
     *   <pre>
     *   {
     *     xScaleFactor: 0.6,
     *     yScaleFactor:0.6,
     *     containerWidth: data.width,
     *     containerHeight: data.height,
     *     position: {
     *       mx: 0.46,
     *       my: 0.2,
     *     }
     *   }
     *   </pre>
     *   <ul>
     *    <li>targetpathwidth = xScaleFactor * containerWidth</li>
     *    <li>targetpathheight = yScaleFactor * containerHeight</li>
     *    <li>Position is used to set the starting coordinate of the path. M is computed:
      *    <ul>
      *      <li>position.x * containerWidth</li>
      *      <li>position.y * containerHeight</li>
      *    </ul>
      *    Center of the container <pre> position: {
     *       mx: 0.5,
     *       my: 0.5,
     *     }</pre>
     *     Upper left corner of the container
     *     <pre> position: {
     *       mx: 0.0,
     *       my: 0.0,
     *     }</pre>
     *    </li>
     *   </ul>
     * </p>
     *
     */


    this.getScaledPath = function getScaledPath(pathId, param) {
      var rawPath = this.pathMap[pathId]; // positioning
      // compute the start point of the path

      var mx, my;

      if (param.abspos) {
        mx = param.abspos.x;
        my = param.abspos.y;
      } else {
        mx = param.containerWidth * param.position.mx;
        my = param.containerHeight * param.position.my;
      }

      var coordinates = {}; // map for the scaled coordinates

      if (param.position) {
        // path
        var heightRatio = param.containerHeight / rawPath.height * param.yScaleFactor;
        var widthRatio = param.containerWidth / rawPath.width * param.xScaleFactor; // Apply height ratio

        for (var heightIndex = 0; heightIndex < rawPath.heightElements.length; heightIndex++) {
          coordinates['y' + heightIndex] = rawPath.heightElements[heightIndex] * heightRatio;
        } // Apply width ratio


        for (var widthIndex = 0; widthIndex < rawPath.widthElements.length; widthIndex++) {
          coordinates['x' + widthIndex] = rawPath.widthElements[widthIndex] * widthRatio;
        }
      } // Apply value to raw path


      var path = format(rawPath.d, {
        mx: mx,
        my: my,
        e: coordinates
      });
      return path;
    };
  } // helpers //////////////////////
  // copied from https://github.com/adobe-webplatform/Snap.svg/blob/master/src/svg.js

  var tokenRegex = /\{([^}]+)\}/g,
      objNotationRegex = /(?:(?:^|\.)(.+?)(?=\[|\.|$|\()|\[('|")(.+?)\2\])(\(\))?/g; // matches .xxxxx or ["xxxxx"] to run over object properties

  function replacer(all, key, obj) {
    var res = obj;
    key.replace(objNotationRegex, function (all, name, quote, quotedName, isFunc) {
      name = name || quotedName;

      if (res) {
        if (name in res) {
          res = res[name];
        }

        typeof res == 'function' && isFunc && (res = res());
      }
    });
    res = (res == null || res == obj ? all : res) + '';
    return res;
  }

  function format(str, obj) {
    return String(str).replace(tokenRegex, function (all, key) {
      return replacer(all, key, obj);
    });
  }

  var DrawModule$1 = {
    __init__: ['drdRenderer'],
    drdRenderer: ['type', DrdRenderer],
    textRenderer: ['type', TextRenderer],
    pathMap: ['type', PathMap]
  };

  function DrdImporter(eventBus, canvas, elementFactory, elementRegistry) {
    this._eventBus = eventBus;
    this._canvas = canvas;
    this._elementRegistry = elementRegistry;
    this._elementFactory = elementFactory;
  }
  DrdImporter.$inject = ['eventBus', 'canvas', 'elementFactory', 'elementRegistry'];

  DrdImporter.prototype.root = function (semantic) {
    var element = this._elementFactory.createRoot(elementData(semantic));

    this._canvas.setRootElement(element);

    return element;
  };
  /**
   * Add drd element (semantic) to the canvas.
   */


  DrdImporter.prototype.add = function (semantic) {
    var elementFactory = this._elementFactory,
        canvas = this._canvas,
        eventBus = this._eventBus,
        di = semantic.di;
    var element, waypoints, source, target, elementDefinition, bounds;

    if (di.$instanceOf('dmndi:DMNShape')) {
      bounds = di.bounds;
      elementDefinition = elementData(semantic, {
        x: Math.round(bounds.x),
        y: Math.round(bounds.y),
        width: Math.round(bounds.width),
        height: Math.round(bounds.height)
      });
      element = elementFactory.createShape(elementDefinition);
      canvas.addShape(element);
      eventBus.fire('drdElement.added', {
        element: element,
        di: di
      });
    } else if (di.$instanceOf('dmndi:DMNEdge')) {
      waypoints = collectWaypoints(di);
      source = this._getSource(semantic);
      target = this._getTarget(semantic);

      if (source && target) {
        elementDefinition = elementData(semantic, {
          hidden: false,
          source: source,
          target: target,
          waypoints: waypoints
        });
        element = elementFactory.createConnection(elementDefinition);
        canvas.addConnection(element);
        eventBus.fire('drdElement.added', {
          element: element,
          di: di
        });
      }
    } else {
      throw new Error('unknown di for element ' + semantic.id);
    }

    return element;
  };

  DrdImporter.prototype._getSource = function (semantic) {
    var href, elementReference;

    if (is(semantic, 'dmn:Association')) {
      elementReference = semantic.sourceRef;
    } else if (is(semantic, 'dmn:InformationRequirement')) {
      elementReference = semantic.requiredDecision || semantic.requiredInput;
    } else if (is(semantic, 'dmn:KnowledgeRequirement')) {
      elementReference = semantic.requiredKnowledge;
    } else if (is(semantic, 'dmn:AuthorityRequirement')) {
      elementReference = semantic.requiredDecision || semantic.requiredInput || semantic.requiredAuthority;
    }

    if (elementReference) {
      href = elementReference.href;
    }

    if (href) {
      return this._getShape(getIdFromHref(href));
    }
  };

  DrdImporter.prototype._getTarget = function (semantic) {
    if (is(semantic, 'dmn:Association')) {
      return semantic.targetRef && this._getShape(getIdFromHref(semantic.targetRef.href));
    }

    return this._getShape(semantic.$parent.id);
  };

  DrdImporter.prototype._getShape = function (id) {
    return this._elementRegistry.get(id);
  }; // helper /////


  function elementData(semantic, attrs) {
    return assign({
      id: semantic.id,
      type: semantic.$type,
      businessObject: semantic
    }, attrs);
  }

  function collectWaypoints(edge) {
    var waypoints = edge.waypoint;

    if (waypoints) {
      return map(waypoints, function (waypoint) {
        var position = {
          x: waypoint.x,
          y: waypoint.y
        };
        return assign({
          original: position
        }, position);
      });
    }
  }

  function getIdFromHref(href) {
    return href.split('#').pop();
  }

  var ImportModule = {
    drdImporter: ['type', DrdImporter]
  };

  var CoreModule$1 = {
    __depends__: [DrawModule$1, ImportModule]
  };

  /**
   * A simple translation stub to be used for multi-language support
   * in diagrams. Can be easily replaced with a more sophisticated
   * solution.
   *
   * @example
   *
   * // use it inside any diagram component by injecting `translate`.
   *
   * function MyService(translate) {
   *   alert(translate('HELLO {you}', { you: 'You!' }));
   * }
   *
   * @param {string} template to interpolate
   * @param {Object} [replacements] a map with substitutes
   *
   * @return {string} the translated string
   */
  function translate$1(template, replacements) {
    replacements = replacements || {};
    return template.replace(/{([^}]+)}/g, function (_, key) {
      return replacements[key] || '{' + key + '}';
    });
  }

  var TranslateModule = {
    translate: ['value', translate$1]
  };

  function getOriginal(event) {
    return event.originalEvent || event.srcEvent;
  }

  function isMac() {
    return /mac/i.test(navigator.platform);
  }

  function isButton(event, button) {
    return (getOriginal(event) || event).button === button;
  }
  function isPrimaryButton(event) {
    // button === 0 -> left áka primary mouse button
    return isButton(event, 0);
  }
  function isAuxiliaryButton(event) {
    // button === 1 -> auxiliary áka wheel button
    return isButton(event, 1);
  }
  function hasPrimaryModifier(event) {
    var originalEvent = getOriginal(event) || event;

    if (!isPrimaryButton(event)) {
      return false;
    } // Use cmd as primary modifier key for mac OS


    if (isMac()) {
      return originalEvent.metaKey;
    } else {
      return originalEvent.ctrlKey;
    }
  }
  function hasSecondaryModifier(event) {
    var originalEvent = getOriginal(event) || event;
    return isPrimaryButton(event) && originalEvent.shiftKey;
  }

  function allowAll(event) {
    return true;
  }

  function allowPrimaryAndAuxiliary(event) {
    return isPrimaryButton(event) || isAuxiliaryButton(event);
  }

  var LOW_PRIORITY = 500;
  /**
   * A plugin that provides interaction events for diagram elements.
   *
   * It emits the following events:
   *
   *   * element.click
   *   * element.contextmenu
   *   * element.dblclick
   *   * element.hover
   *   * element.mousedown
   *   * element.mousemove
   *   * element.mouseup
   *   * element.out
   *
   * Each event is a tuple { element, gfx, originalEvent }.
   *
   * Canceling the event via Event#preventDefault()
   * prevents the original DOM operation.
   *
   * @param {EventBus} eventBus
   */

  function InteractionEvents(eventBus, elementRegistry, styles) {
    var self = this;
    /**
     * Fire an interaction event.
     *
     * @param {string} type local event name, e.g. element.click.
     * @param {DOMEvent} event native event
     * @param {djs.model.Base} [element] the diagram element to emit the event on;
     *                                   defaults to the event target
     */

    function fire(type, event, element) {
      if (isIgnored(type, event)) {
        return;
      }

      var target, gfx, returnValue;

      if (!element) {
        target = event.delegateTarget || event.target;

        if (target) {
          gfx = target;
          element = elementRegistry.get(gfx);
        }
      } else {
        gfx = elementRegistry.getGraphics(element);
      }

      if (!gfx || !element) {
        return;
      }

      returnValue = eventBus.fire(type, {
        element: element,
        gfx: gfx,
        originalEvent: event
      });

      if (returnValue === false) {
        event.stopPropagation();
        event.preventDefault();
      }
    } // TODO(nikku): document this


    var handlers = {};

    function mouseHandler(localEventName) {
      return handlers[localEventName];
    }

    function isIgnored(localEventName, event) {
      var filter = ignoredFilters[localEventName] || isPrimaryButton; // only react on left mouse button interactions
      // except for interaction events that are enabled
      // for secundary mouse button

      return !filter(event);
    }

    var bindings = {
      click: 'element.click',
      contextmenu: 'element.contextmenu',
      dblclick: 'element.dblclick',
      mousedown: 'element.mousedown',
      mousemove: 'element.mousemove',
      mouseover: 'element.hover',
      mouseout: 'element.out',
      mouseup: 'element.mouseup'
    };
    var ignoredFilters = {
      'element.contextmenu': allowAll,
      'element.mousedown': allowPrimaryAndAuxiliary,
      'element.mouseup': allowPrimaryAndAuxiliary,
      'element.click': allowPrimaryAndAuxiliary,
      'element.dblclick': allowPrimaryAndAuxiliary
    }; // manual event trigger //////////

    /**
     * Trigger an interaction event (based on a native dom event)
     * on the target shape or connection.
     *
     * @param {string} eventName the name of the triggered DOM event
     * @param {MouseEvent} event
     * @param {djs.model.Base} targetElement
     */

    function triggerMouseEvent(eventName, event, targetElement) {
      // i.e. element.mousedown...
      var localEventName = bindings[eventName];

      if (!localEventName) {
        throw new Error('unmapped DOM event name <' + eventName + '>');
      }

      return fire(localEventName, event, targetElement);
    }

    var ELEMENT_SELECTOR = 'svg, .djs-element'; // event handling ///////

    function registerEvent(node, event, localEvent, ignoredFilter) {
      var handler = handlers[localEvent] = function (event) {
        fire(localEvent, event);
      };

      if (ignoredFilter) {
        ignoredFilters[localEvent] = ignoredFilter;
      }

      handler.$delegate = delegate.bind(node, ELEMENT_SELECTOR, event, handler);
    }

    function unregisterEvent(node, event, localEvent) {
      var handler = mouseHandler(localEvent);

      if (!handler) {
        return;
      }

      delegate.unbind(node, event, handler.$delegate);
    }

    function registerEvents(svg) {
      forEach(bindings, function (val, key) {
        registerEvent(svg, key, val);
      });
    }

    function unregisterEvents(svg) {
      forEach(bindings, function (val, key) {
        unregisterEvent(svg, key, val);
      });
    }

    eventBus.on('canvas.destroy', function (event) {
      unregisterEvents(event.svg);
    });
    eventBus.on('canvas.init', function (event) {
      registerEvents(event.svg);
    }); // hit box updating ////////////////

    eventBus.on(['shape.added', 'connection.added'], function (event) {
      var element = event.element,
          gfx = event.gfx;
      eventBus.fire('interactionEvents.createHit', {
        element: element,
        gfx: gfx
      });
    }); // Update djs-hit on change.
    // A low priortity is necessary, because djs-hit of labels has to be updated
    // after the label bounds have been updated in the renderer.

    eventBus.on(['shape.changed', 'connection.changed'], LOW_PRIORITY, function (event) {
      var element = event.element,
          gfx = event.gfx;
      eventBus.fire('interactionEvents.updateHit', {
        element: element,
        gfx: gfx
      });
    });
    eventBus.on('interactionEvents.createHit', LOW_PRIORITY, function (event) {
      var element = event.element,
          gfx = event.gfx;
      self.createDefaultHit(element, gfx);
    });
    eventBus.on('interactionEvents.updateHit', function (event) {
      var element = event.element,
          gfx = event.gfx;
      self.updateDefaultHit(element, gfx);
    }); // hit styles ////////////

    var STROKE_HIT_STYLE = createHitStyle('djs-hit djs-hit-stroke');
    var CLICK_STROKE_HIT_STYLE = createHitStyle('djs-hit djs-hit-click-stroke');
    var ALL_HIT_STYLE = createHitStyle('djs-hit djs-hit-all');
    var HIT_TYPES = {
      'all': ALL_HIT_STYLE,
      'click-stroke': CLICK_STROKE_HIT_STYLE,
      'stroke': STROKE_HIT_STYLE
    };

    function createHitStyle(classNames, attrs) {
      attrs = assign({
        stroke: 'white',
        strokeWidth: 15
      }, attrs || {});
      return styles.cls(classNames, ['no-fill', 'no-border'], attrs);
    } // style helpers ///////////////


    function applyStyle(hit, type) {
      var attrs = HIT_TYPES[type];

      if (!attrs) {
        throw new Error('invalid hit type <' + type + '>');
      }

      attr$1(hit, attrs);
      return hit;
    }

    function appendHit(gfx, hit) {
      append(gfx, hit);
    } // API

    /**
     * Remove hints on the given graphics.
     *
     * @param {SVGElement} gfx
     */


    this.removeHits = function (gfx) {
      var hits = all('.djs-hit', gfx);
      forEach(hits, remove$1);
    };
    /**
     * Create default hit for the given element.
     *
     * @param {djs.model.Base} element
     * @param {SVGElement} gfx
     *
     * @return {SVGElement} created hit
     */


    this.createDefaultHit = function (element, gfx) {
      var waypoints = element.waypoints,
          isFrame = element.isFrame,
          boxType;

      if (waypoints) {
        return this.createWaypointsHit(gfx, waypoints);
      } else {
        boxType = isFrame ? 'stroke' : 'all';
        return this.createBoxHit(gfx, boxType, {
          width: element.width,
          height: element.height
        });
      }
    };
    /**
     * Create hits for the given waypoints.
     *
     * @param {SVGElement} gfx
     * @param {Array<Point>} waypoints
     *
     * @return {SVGElement}
     */


    this.createWaypointsHit = function (gfx, waypoints) {
      var hit = createLine(waypoints);
      applyStyle(hit, 'stroke');
      appendHit(gfx, hit);
      return hit;
    };
    /**
     * Create hits for a box.
     *
     * @param {SVGElement} gfx
     * @param {string} hitType
     * @param {Object} attrs
     *
     * @return {SVGElement}
     */


    this.createBoxHit = function (gfx, type, attrs) {
      attrs = assign({
        x: 0,
        y: 0
      }, attrs);
      var hit = create('rect');
      applyStyle(hit, type);
      attr$1(hit, attrs);
      appendHit(gfx, hit);
      return hit;
    };
    /**
     * Update default hit of the element.
     *
     * @param  {djs.model.Base} element
     * @param  {SVGElement} gfx
     *
     * @return {SVGElement} updated hit
     */


    this.updateDefaultHit = function (element, gfx) {
      var hit = query('.djs-hit', gfx);

      if (!hit) {
        return;
      }

      if (element.waypoints) {
        updateLine(hit, element.waypoints);
      } else {
        attr$1(hit, {
          width: element.width,
          height: element.height
        });
      }

      return hit;
    };

    this.fire = fire;
    this.triggerMouseEvent = triggerMouseEvent;
    this.mouseHandler = mouseHandler;
    this.registerEvent = registerEvent;
    this.unregisterEvent = unregisterEvent;
  }
  InteractionEvents.$inject = ['eventBus', 'elementRegistry', 'styles'];
  /**
   * An event indicating that the mouse hovered over an element
   *
   * @event element.hover
   *
   * @type {Object}
   * @property {djs.model.Base} element
   * @property {SVGElement} gfx
   * @property {Event} originalEvent
   */

  /**
   * An event indicating that the mouse has left an element
   *
   * @event element.out
   *
   * @type {Object}
   * @property {djs.model.Base} element
   * @property {SVGElement} gfx
   * @property {Event} originalEvent
   */

  /**
   * An event indicating that the mouse has clicked an element
   *
   * @event element.click
   *
   * @type {Object}
   * @property {djs.model.Base} element
   * @property {SVGElement} gfx
   * @property {Event} originalEvent
   */

  /**
   * An event indicating that the mouse has double clicked an element
   *
   * @event element.dblclick
   *
   * @type {Object}
   * @property {djs.model.Base} element
   * @property {SVGElement} gfx
   * @property {Event} originalEvent
   */

  /**
   * An event indicating that the mouse has gone down on an element.
   *
   * @event element.mousedown
   *
   * @type {Object}
   * @property {djs.model.Base} element
   * @property {SVGElement} gfx
   * @property {Event} originalEvent
   */

  /**
   * An event indicating that the mouse has gone up on an element.
   *
   * @event element.mouseup
   *
   * @type {Object}
   * @property {djs.model.Base} element
   * @property {SVGElement} gfx
   * @property {Event} originalEvent
   */

  /**
   * An event indicating that the context menu action is triggered
   * via mouse or touch controls.
   *
   * @event element.contextmenu
   *
   * @type {Object}
   * @property {djs.model.Base} element
   * @property {SVGElement} gfx
   * @property {Event} originalEvent
   */

  var InteractionEventsModule = {
    __init__: ['interactionEvents'],
    interactionEvents: ['type', InteractionEvents]
  };

  var LOW_PRIORITY$1 = 500;
  /**
   * @class
   *
   * A plugin that adds an outline to shapes and connections that may be activated and styled
   * via CSS classes.
   *
   * @param {EventBus} eventBus
   * @param {Styles} styles
   * @param {ElementRegistry} elementRegistry
   */

  function Outline(eventBus, styles, elementRegistry) {
    this.offset = 6;
    var OUTLINE_STYLE = styles.cls('djs-outline', ['no-fill']);
    var self = this;

    function createOutline(gfx, bounds) {
      var outline = create('rect');
      attr$1(outline, assign({
        x: 10,
        y: 10,
        width: 100,
        height: 100
      }, OUTLINE_STYLE));
      append(gfx, outline);
      return outline;
    } // A low priortity is necessary, because outlines of labels have to be updated
    // after the label bounds have been updated in the renderer.


    eventBus.on(['shape.added', 'shape.changed'], LOW_PRIORITY$1, function (event) {
      var element = event.element,
          gfx = event.gfx;
      var outline = query('.djs-outline', gfx);

      if (!outline) {
        outline = createOutline(gfx);
      }

      self.updateShapeOutline(outline, element);
    });
    eventBus.on(['connection.added', 'connection.changed'], function (event) {
      var element = event.element,
          gfx = event.gfx;
      var outline = query('.djs-outline', gfx);

      if (!outline) {
        outline = createOutline(gfx);
      }

      self.updateConnectionOutline(outline, element);
    });
  }
  /**
   * Updates the outline of a shape respecting the dimension of the
   * element and an outline offset.
   *
   * @param  {SVGElement} outline
   * @param  {djs.model.Base} element
   */

  Outline.prototype.updateShapeOutline = function (outline, element) {
    attr$1(outline, {
      x: -this.offset,
      y: -this.offset,
      width: element.width + this.offset * 2,
      height: element.height + this.offset * 2
    });
  };
  /**
   * Updates the outline of a connection respecting the bounding box of
   * the connection and an outline offset.
   *
   * @param  {SVGElement} outline
   * @param  {djs.model.Base} element
   */


  Outline.prototype.updateConnectionOutline = function (outline, connection) {
    var bbox = getBBox(connection);
    attr$1(outline, {
      x: bbox.x - this.offset,
      y: bbox.y - this.offset,
      width: bbox.width + this.offset * 2,
      height: bbox.height + this.offset * 2
    });
  };

  Outline.$inject = ['eventBus', 'styles', 'elementRegistry'];

  var OutlineModule = {
    __init__: ['outline'],
    outline: ['type', Outline]
  };

  /**
   * A service that offers the current selection in a diagram.
   * Offers the api to control the selection, too.
   *
   * @class
   *
   * @param {EventBus} eventBus the event bus
   */

  function Selection(eventBus) {
    this._eventBus = eventBus;
    this._selectedElements = [];
    var self = this;
    eventBus.on(['shape.remove', 'connection.remove'], function (e) {
      var element = e.element;
      self.deselect(element);
    });
    eventBus.on(['diagram.clear'], function (e) {
      self.select(null);
    });
  }
  Selection.$inject = ['eventBus'];

  Selection.prototype.deselect = function (element) {
    var selectedElements = this._selectedElements;
    var idx = selectedElements.indexOf(element);

    if (idx !== -1) {
      var oldSelection = selectedElements.slice();
      selectedElements.splice(idx, 1);

      this._eventBus.fire('selection.changed', {
        oldSelection: oldSelection,
        newSelection: selectedElements
      });
    }
  };

  Selection.prototype.get = function () {
    return this._selectedElements;
  };

  Selection.prototype.isSelected = function (element) {
    return this._selectedElements.indexOf(element) !== -1;
  };
  /**
   * This method selects one or more elements on the diagram.
   *
   * By passing an additional add parameter you can decide whether or not the element(s)
   * should be added to the already existing selection or not.
   *
   * @method Selection#select
   *
   * @param  {Object|Object[]} elements element or array of elements to be selected
   * @param  {boolean} [add] whether the element(s) should be appended to the current selection, defaults to false
   */


  Selection.prototype.select = function (elements, add) {
    var selectedElements = this._selectedElements,
        oldSelection = selectedElements.slice();

    if (!isArray(elements)) {
      elements = elements ? [elements] : [];
    } // selection may be cleared by passing an empty array or null
    // to the method


    if (add) {
      forEach(elements, function (element) {
        if (selectedElements.indexOf(element) !== -1) {
          // already selected
          return;
        } else {
          selectedElements.push(element);
        }
      });
    } else {
      this._selectedElements = selectedElements = elements.slice();
    }

    this._eventBus.fire('selection.changed', {
      oldSelection: oldSelection,
      newSelection: selectedElements
    });
  };

  var MARKER_HOVER = 'hover',
      MARKER_SELECTED = 'selected';
  /**
   * A plugin that adds a visible selection UI to shapes and connections
   * by appending the <code>hover</code> and <code>selected</code> classes to them.
   *
   * @class
   *
   * Makes elements selectable, too.
   *
   * @param {EventBus} events
   * @param {SelectionService} selection
   * @param {Canvas} canvas
   */

  function SelectionVisuals(events, canvas, selection, styles) {
    this._multiSelectionBox = null;

    function addMarker(e, cls) {
      canvas.addMarker(e, cls);
    }

    function removeMarker(e, cls) {
      canvas.removeMarker(e, cls);
    }

    events.on('element.hover', function (event) {
      addMarker(event.element, MARKER_HOVER);
    });
    events.on('element.out', function (event) {
      removeMarker(event.element, MARKER_HOVER);
    });
    events.on('selection.changed', function (event) {
      function deselect(s) {
        removeMarker(s, MARKER_SELECTED);
      }

      function select(s) {
        addMarker(s, MARKER_SELECTED);
      }

      var oldSelection = event.oldSelection,
          newSelection = event.newSelection;
      forEach(oldSelection, function (e) {
        if (newSelection.indexOf(e) === -1) {
          deselect(e);
        }
      });
      forEach(newSelection, function (e) {
        if (oldSelection.indexOf(e) === -1) {
          select(e);
        }
      });
    });
  }
  SelectionVisuals.$inject = ['eventBus', 'canvas', 'selection', 'styles'];

  function SelectionBehavior(eventBus, selection, canvas, elementRegistry) {
    // Select elements on create
    eventBus.on('create.end', 500, function (event) {
      var context = event.context,
          canExecute = context.canExecute,
          elements = context.elements,
          hints = context.hints || {},
          autoSelect = hints.autoSelect;

      if (canExecute) {
        if (autoSelect === false) {
          // Select no elements
          return;
        }

        if (isArray(autoSelect)) {
          selection.select(autoSelect);
        } else {
          // Select all elements by default
          selection.select(elements.filter(isShown));
        }
      }
    }); // Select connection targets on connect

    eventBus.on('connect.end', 500, function (event) {
      var context = event.context,
          canExecute = context.canExecute,
          hover = context.hover;

      if (canExecute && hover) {
        selection.select(hover);
      }
    }); // Select shapes on move

    eventBus.on('shape.move.end', 500, function (event) {
      var previousSelection = event.previousSelection || [];
      var shape = elementRegistry.get(event.context.shape.id); // Always select main shape on move

      var isSelected = find(previousSelection, function (selectedShape) {
        return shape.id === selectedShape.id;
      });

      if (!isSelected) {
        selection.select(shape);
      }
    }); // Select elements on click

    eventBus.on('element.click', function (event) {
      if (!isPrimaryButton(event)) {
        return;
      }

      var element = event.element;

      if (element === canvas.getRootElement()) {
        element = null;
      }

      var isSelected = selection.isSelected(element),
          isMultiSelect = selection.get().length > 1; // Add to selection if CTRL or SHIFT pressed

      var add = hasPrimaryModifier(event) || hasSecondaryModifier(event);

      if (isSelected && isMultiSelect) {
        if (add) {
          // Deselect element
          return selection.deselect(element);
        } else {
          // Select element only
          return selection.select(element);
        }
      } else if (!isSelected) {
        // Select element
        selection.select(element, add);
      } else {
        // Deselect element
        selection.deselect(element);
      }
    });
  }
  SelectionBehavior.$inject = ['eventBus', 'selection', 'canvas', 'elementRegistry'];

  function isShown(element) {
    return !element.hidden;
  }

  var SelectionModule = {
    __init__: ['selectionVisuals', 'selectionBehavior'],
    __depends__: [InteractionEventsModule, OutlineModule],
    selection: ['type', Selection],
    selectionVisuals: ['type', SelectionVisuals],
    selectionBehavior: ['type', SelectionBehavior]
  };

  /**
   * Util that provides unique IDs.
   *
   * @class djs.util.IdGenerator
   * @constructor
   * @memberOf djs.util
   *
   * The ids can be customized via a given prefix and contain a random value to avoid collisions.
   *
   * @param {string} prefix a prefix to prepend to generated ids (for better readability)
   */
  function IdGenerator(prefix) {
    this._counter = 0;
    this._prefix = (prefix ? prefix + '-' : '') + Math.floor(Math.random() * 1000000000) + '-';
  }
  /**
   * Returns a next unique ID.
   *
   * @method djs.util.IdGenerator#next
   *
   * @returns {string} the id
   */

  IdGenerator.prototype.next = function () {
    return this._prefix + ++this._counter;
  };

  var ids = new IdGenerator('ov');
  var LOW_PRIORITY$2 = 500;
  /**
   * A service that allows users to attach overlays to diagram elements.
   *
   * The overlay service will take care of overlay positioning during updates.
   *
   * @example
   *
   * // add a pink badge on the top left of the shape
   * overlays.add(someShape, {
   *   position: {
   *     top: -5,
   *     left: -5
   *   },
   *   html: '<div style="width: 10px; background: fuchsia; color: white;">0</div>'
   * });
   *
   * // or add via shape id
   *
   * overlays.add('some-element-id', {
   *   position: {
   *     top: -5,
   *     left: -5
   *   }
   *   html: '<div style="width: 10px; background: fuchsia; color: white;">0</div>'
   * });
   *
   * // or add with optional type
   *
   * overlays.add(someShape, 'badge', {
   *   position: {
   *     top: -5,
   *     left: -5
   *   }
   *   html: '<div style="width: 10px; background: fuchsia; color: white;">0</div>'
   * });
   *
   *
   * // remove an overlay
   *
   * var id = overlays.add(...);
   * overlays.remove(id);
   *
   *
   * You may configure overlay defaults during tool by providing a `config` module
   * with `overlays.defaults` as an entry:
   *
   * {
   *   overlays: {
   *     defaults: {
   *       show: {
   *         minZoom: 0.7,
   *         maxZoom: 5.0
   *       },
   *       scale: {
   *         min: 1
   *       }
   *     }
   * }
   *
   * @param {Object} config
   * @param {EventBus} eventBus
   * @param {Canvas} canvas
   * @param {ElementRegistry} elementRegistry
   */

  function Overlays(config, eventBus, canvas, elementRegistry) {
    this._eventBus = eventBus;
    this._canvas = canvas;
    this._elementRegistry = elementRegistry;
    this._ids = ids;
    this._overlayDefaults = assign({
      // no show constraints
      show: null,
      // always scale
      scale: true
    }, config && config.defaults);
    /**
     * Mapping overlayId -> overlay
     */

    this._overlays = {};
    /**
     * Mapping elementId -> overlay container
     */

    this._overlayContainers = []; // root html element for all overlays

    this._overlayRoot = createRoot(canvas.getContainer());

    this._init();
  }
  Overlays.$inject = ['config.overlays', 'eventBus', 'canvas', 'elementRegistry'];
  /**
   * Returns the overlay with the specified id or a list of overlays
   * for an element with a given type.
   *
   * @example
   *
   * // return the single overlay with the given id
   * overlays.get('some-id');
   *
   * // return all overlays for the shape
   * overlays.get({ element: someShape });
   *
   * // return all overlays on shape with type 'badge'
   * overlays.get({ element: someShape, type: 'badge' });
   *
   * // shape can also be specified as id
   * overlays.get({ element: 'element-id', type: 'badge' });
   *
   *
   * @param {Object} search
   * @param {string} [search.id]
   * @param {string|djs.model.Base} [search.element]
   * @param {string} [search.type]
   *
   * @return {Object|Array<Object>} the overlay(s)
   */

  Overlays.prototype.get = function (search) {
    if (isString(search)) {
      search = {
        id: search
      };
    }

    if (isString(search.element)) {
      search.element = this._elementRegistry.get(search.element);
    }

    if (search.element) {
      var container = this._getOverlayContainer(search.element, true); // return a list of overlays when searching by element (+type)


      if (container) {
        return search.type ? filter(container.overlays, matchPattern({
          type: search.type
        })) : container.overlays.slice();
      } else {
        return [];
      }
    } else if (search.type) {
      return filter(this._overlays, matchPattern({
        type: search.type
      }));
    } else {
      // return single element when searching by id
      return search.id ? this._overlays[search.id] : null;
    }
  };
  /**
   * Adds a HTML overlay to an element.
   *
   * @param {string|djs.model.Base}   element   attach overlay to this shape
   * @param {string}                  [type]    optional type to assign to the overlay
   * @param {Object}                  overlay   the overlay configuration
   *
   * @param {string|DOMElement}       overlay.html                 html element to use as an overlay
   * @param {Object}                  [overlay.show]               show configuration
   * @param {number}                  [overlay.show.minZoom]       minimal zoom level to show the overlay
   * @param {number}                  [overlay.show.maxZoom]       maximum zoom level to show the overlay
   * @param {Object}                  overlay.position             where to attach the overlay
   * @param {number}                  [overlay.position.left]      relative to element bbox left attachment
   * @param {number}                  [overlay.position.top]       relative to element bbox top attachment
   * @param {number}                  [overlay.position.bottom]    relative to element bbox bottom attachment
   * @param {number}                  [overlay.position.right]     relative to element bbox right attachment
   * @param {boolean|Object}          [overlay.scale=true]         false to preserve the same size regardless of
   *                                                               diagram zoom
   * @param {number}                  [overlay.scale.min]
   * @param {number}                  [overlay.scale.max]
   *
   * @return {string}                 id that may be used to reference the overlay for update or removal
   */


  Overlays.prototype.add = function (element, type, overlay) {
    if (isObject(type)) {
      overlay = type;
      type = null;
    }

    if (!element.id) {
      element = this._elementRegistry.get(element);
    }

    if (!overlay.position) {
      throw new Error('must specifiy overlay position');
    }

    if (!overlay.html) {
      throw new Error('must specifiy overlay html');
    }

    if (!element) {
      throw new Error('invalid element specified');
    }

    var id = this._ids.next();

    overlay = assign({}, this._overlayDefaults, overlay, {
      id: id,
      type: type,
      element: element,
      html: overlay.html
    });

    this._addOverlay(overlay);

    return id;
  };
  /**
   * Remove an overlay with the given id or all overlays matching the given filter.
   *
   * @see Overlays#get for filter options.
   *
   * @param {string} [id]
   * @param {Object} [filter]
   */


  Overlays.prototype.remove = function (filter) {
    var overlays = this.get(filter) || [];

    if (!isArray(overlays)) {
      overlays = [overlays];
    }

    var self = this;
    forEach(overlays, function (overlay) {
      var container = self._getOverlayContainer(overlay.element, true);

      if (overlay) {
        remove(overlay.html);
        remove(overlay.htmlContainer);
        delete overlay.htmlContainer;
        delete overlay.element;
        delete self._overlays[overlay.id];
      }

      if (container) {
        var idx = container.overlays.indexOf(overlay);

        if (idx !== -1) {
          container.overlays.splice(idx, 1);
        }
      }
    });
  };

  Overlays.prototype.show = function () {
    setVisible(this._overlayRoot);
  };

  Overlays.prototype.hide = function () {
    setVisible(this._overlayRoot, false);
  };

  Overlays.prototype.clear = function () {
    this._overlays = {};
    this._overlayContainers = [];
    clear(this._overlayRoot);
  };

  Overlays.prototype._updateOverlayContainer = function (container) {
    var element = container.element,
        html = container.html; // update container left,top according to the elements x,y coordinates
    // this ensures we can attach child elements relative to this container

    var x = element.x,
        y = element.y;

    if (element.waypoints) {
      var bbox = getBBox(element);
      x = bbox.x;
      y = bbox.y;
    }

    setPosition(html, x, y);
    attr(container.html, 'data-container-id', element.id);
  };

  Overlays.prototype._updateOverlay = function (overlay) {
    var position = overlay.position,
        htmlContainer = overlay.htmlContainer,
        element = overlay.element; // update overlay html relative to shape because
    // it is already positioned on the element
    // update relative

    var left = position.left,
        top = position.top;

    if (position.right !== undefined) {
      var width;

      if (element.waypoints) {
        width = getBBox(element).width;
      } else {
        width = element.width;
      }

      left = position.right * -1 + width;
    }

    if (position.bottom !== undefined) {
      var height;

      if (element.waypoints) {
        height = getBBox(element).height;
      } else {
        height = element.height;
      }

      top = position.bottom * -1 + height;
    }

    setPosition(htmlContainer, left || 0, top || 0);
  };

  Overlays.prototype._createOverlayContainer = function (element) {
    var html = domify('<div class="djs-overlays" style="position: absolute" />');

    this._overlayRoot.appendChild(html);

    var container = {
      html: html,
      element: element,
      overlays: []
    };

    this._updateOverlayContainer(container);

    this._overlayContainers.push(container);

    return container;
  };

  Overlays.prototype._updateRoot = function (viewbox) {
    var scale = viewbox.scale || 1;
    var matrix = 'matrix(' + [scale, 0, 0, scale, -1 * viewbox.x * scale, -1 * viewbox.y * scale].join(',') + ')';
    setTransform(this._overlayRoot, matrix);
  };

  Overlays.prototype._getOverlayContainer = function (element, raw) {
    var container = find(this._overlayContainers, function (c) {
      return c.element === element;
    });

    if (!container && !raw) {
      return this._createOverlayContainer(element);
    }

    return container;
  };

  Overlays.prototype._addOverlay = function (overlay) {
    var id = overlay.id,
        element = overlay.element,
        html = overlay.html,
        htmlContainer,
        overlayContainer; // unwrap jquery (for those who need it)

    if (html.get && html.constructor.prototype.jquery) {
      html = html.get(0);
    } // create proper html elements from
    // overlay HTML strings


    if (isString(html)) {
      html = domify(html);
    }

    overlayContainer = this._getOverlayContainer(element);
    htmlContainer = domify('<div class="djs-overlay" data-overlay-id="' + id + '" style="position: absolute">');
    htmlContainer.appendChild(html);

    if (overlay.type) {
      classes(htmlContainer).add('djs-overlay-' + overlay.type);
    }

    overlay.htmlContainer = htmlContainer;
    overlayContainer.overlays.push(overlay);
    overlayContainer.html.appendChild(htmlContainer);
    this._overlays[id] = overlay;

    this._updateOverlay(overlay);

    this._updateOverlayVisibilty(overlay, this._canvas.viewbox());
  };

  Overlays.prototype._updateOverlayVisibilty = function (overlay, viewbox) {
    var show = overlay.show,
        minZoom = show && show.minZoom,
        maxZoom = show && show.maxZoom,
        htmlContainer = overlay.htmlContainer,
        visible = true;

    if (show) {
      if (isDefined(minZoom) && minZoom > viewbox.scale || isDefined(maxZoom) && maxZoom < viewbox.scale) {
        visible = false;
      }

      setVisible(htmlContainer, visible);
    }

    this._updateOverlayScale(overlay, viewbox);
  };

  Overlays.prototype._updateOverlayScale = function (overlay, viewbox) {
    var shouldScale = overlay.scale,
        minScale,
        maxScale,
        htmlContainer = overlay.htmlContainer;
    var scale,
        transform = '';

    if (shouldScale !== true) {
      if (shouldScale === false) {
        minScale = 1;
        maxScale = 1;
      } else {
        minScale = shouldScale.min;
        maxScale = shouldScale.max;
      }

      if (isDefined(minScale) && viewbox.scale < minScale) {
        scale = (1 / viewbox.scale || 1) * minScale;
      }

      if (isDefined(maxScale) && viewbox.scale > maxScale) {
        scale = (1 / viewbox.scale || 1) * maxScale;
      }
    }

    if (isDefined(scale)) {
      transform = 'scale(' + scale + ',' + scale + ')';
    }

    setTransform(htmlContainer, transform);
  };

  Overlays.prototype._updateOverlaysVisibilty = function (viewbox) {
    var self = this;
    forEach(this._overlays, function (overlay) {
      self._updateOverlayVisibilty(overlay, viewbox);
    });
  };

  Overlays.prototype._init = function () {
    var eventBus = this._eventBus;
    var self = this; // scroll/zoom integration

    function updateViewbox(viewbox) {
      self._updateRoot(viewbox);

      self._updateOverlaysVisibilty(viewbox);

      self.show();
    }

    eventBus.on('canvas.viewbox.changing', function (event) {
      self.hide();
    });
    eventBus.on('canvas.viewbox.changed', function (event) {
      updateViewbox(event.viewbox);
    }); // remove integration

    eventBus.on(['shape.remove', 'connection.remove'], function (e) {
      var element = e.element;
      var overlays = self.get({
        element: element
      });
      forEach(overlays, function (o) {
        self.remove(o.id);
      });

      var container = self._getOverlayContainer(element);

      if (container) {
        remove(container.html);

        var i = self._overlayContainers.indexOf(container);

        if (i !== -1) {
          self._overlayContainers.splice(i, 1);
        }
      }
    }); // move integration

    eventBus.on('element.changed', LOW_PRIORITY$2, function (e) {
      var element = e.element;

      var container = self._getOverlayContainer(element, true);

      if (container) {
        forEach(container.overlays, function (overlay) {
          self._updateOverlay(overlay);
        });

        self._updateOverlayContainer(container);
      }
    }); // marker integration, simply add them on the overlays as classes, too.

    eventBus.on('element.marker.update', function (e) {
      var container = self._getOverlayContainer(e.element, true);

      if (container) {
        classes(container.html)[e.add ? 'add' : 'remove'](e.marker);
      }
    }); // clear overlays with diagram

    eventBus.on('diagram.clear', this.clear, this);
  }; // helpers /////////////////////////////


  function createRoot(parentNode) {
    var root = domify('<div class="djs-overlay-container" style="position: absolute; width: 0; height: 0;" />');
    parentNode.insertBefore(root, parentNode.firstChild);
    return root;
  }

  function setPosition(el, x, y) {
    assign(el.style, {
      left: x + 'px',
      top: y + 'px'
    });
  }

  function setVisible(el, visible) {
    el.style.display = visible === false ? 'none' : '';
  }

  function setTransform(el, transform) {
    el.style['transform-origin'] = 'top left';
    ['', '-ms-', '-webkit-'].forEach(function (prefix) {
      el.style[prefix + 'transform'] = transform;
    });
  }

  var OverlaysModule = {
    __init__: ['overlays'],
    overlays: ['type', Overlays]
  };

  function DefinitionPropertiesView(eventBus, canvas) {
    this._eventBus = eventBus;
    this._canvas = canvas;
    eventBus.on('diagram.init', function () {
      this._init();
    }, this);
    eventBus.on('import.done', function (event) {
      if (!event.error) {
        this.update();
      }
    }, this);
  }
  DefinitionPropertiesView.$inject = ['eventBus', 'canvas'];
  /**
   * Initialize
   */

  DefinitionPropertiesView.prototype._init = function () {
    var canvas = this._canvas,
        eventBus = this._eventBus;
    var parent = canvas.getContainer(),
        container = this._container = domify(DefinitionPropertiesView.HTML_MARKUP);
    parent.appendChild(container);
    this.nameElement = query('.dmn-definitions-name', this._container);
    this.idElement = query('.dmn-definitions-id', this._container);
    delegate.bind(container, '.dmn-definitions-name, .dmn-definitions-id', 'mousedown', function (event) {
      event.stopPropagation();
    });
    eventBus.fire('definitionIdView.create', {
      html: container
    });
  };

  DefinitionPropertiesView.prototype.update = function () {
    var businessObject = this._canvas.getRootElement().businessObject;

    this.nameElement.textContent = businessObject.name;
    this.idElement.textContent = businessObject.id;
  };
  /* markup definition */


  DefinitionPropertiesView.HTML_MARKUP = '<div class="dmn-definitions">' + '<div class="dmn-definitions-name" title="Definition Name"></div>' + '<div class="dmn-definitions-id" title="Definition ID"></div>' + '</div>';

  function PaletteAdapter(eventBus, canvas) {
    function toggleMarker(cls, on) {
      var container = canvas.getContainer();
      classes(container).toggle(cls, on);
    }

    eventBus.on('palette.create', function () {
      toggleMarker('with-palette', true);
    });
    eventBus.on('palette.changed', function (event) {
      toggleMarker('with-palette-two-column', event.twoColumn);
    });
  }
  PaletteAdapter.$inject = ['eventBus', 'canvas'];

  var DefinitionPropertiesModule = {
    __init__: ['definitionPropertiesView', 'definitionPropertiesPaletteAdapter'],
    definitionPropertiesView: ['type', DefinitionPropertiesView],
    definitionPropertiesPaletteAdapter: ['type', PaletteAdapter]
  };

  function _classCallCheck$2(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$2(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$2(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$2(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$2(Constructor, staticProps);
    return Constructor;
  }
  var PROVIDERS = [{
    className: 'dmn-icon-decision-table',
    matches: function matches(el) {
      var businessObject = el.businessObject;
      return is(businessObject, 'dmn:Decision') && is(businessObject.decisionLogic, 'dmn:DecisionTable');
    }
  }, {
    className: 'dmn-icon-literal-expression',
    matches: function matches(el) {
      var businessObject = el.businessObject;
      return is(businessObject, 'dmn:Decision') && is(businessObject.decisionLogic, 'dmn:LiteralExpression');
    }
  }];
  /**
   * Displays overlays that can be clicked in order to drill
   * down into a DMN element.
   */

  var DrillDown =
  /*#__PURE__*/
  function () {
    function DrillDown(injector, eventBus, overlays, config) {
      var _this = this;

      _classCallCheck$2(this, DrillDown);

      this._injector = injector;
      this._eventBus = eventBus;
      this._overlays = overlays;
      this._config = config || {
        enabled: true
      };
      eventBus.on(['shape.added'], function (_ref) {
        var element = _ref.element;

        for (var i = 0; i < PROVIDERS.length; i++) {
          var _PROVIDERS$i = PROVIDERS[i],
              matches = _PROVIDERS$i.matches,
              className = _PROVIDERS$i.className;
          var editable = matches && matches(element);

          if (editable) {
            _this.addOverlay(element, className);
          }
        }
      });
    }
    /**
     * Add overlay to an element that enables drill down.
     *
     * @param {Object} element Element to add overlay to.
     * @param {string} className
     *        CSS class that will be added to overlay in order to display icon.
     */


    _createClass$2(DrillDown, [{
      key: "addOverlay",
      value: function addOverlay(element, className) {
        var html = domify("\n      <div class=\"drill-down-overlay\">\n        <span class=\"".concat(className, "\"></span>\n      </div>\n    "));

        var overlayId = this._overlays.add(element, {
          position: {
            top: 2,
            left: 2
          },
          html: html
        }); // TODO(nikku): can we remove renamed to drillDown.enabled


        if (this._config.enabled !== false) {
          classes(html).add('interactive');
          this.bindEventListener(element, html, overlayId);
        }
      }
      /**
       * @param {Object} element
       * @param {Object} overlay
       * @param {string} id
       */

    }, {
      key: "bindEventListener",
      value: function bindEventListener(element, overlay, id) {
        var _this2 = this;

        var overlays = this._overlays,
            eventBus = this._eventBus;
        var overlaysRoot = overlays._overlayRoot;
        delegate.bind(overlaysRoot, '[data-overlay-id="' + id + '"]', 'click', function () {
          var triggerDefault = eventBus.fire('drillDown.click', {
            element: element
          });

          if (triggerDefault === false) {
            return;
          }

          _this2.drillDown(element);
        });
      }
      /**
       * Drill down into the specific element.
       *
       * @param  {djs.model.Base} element
       *
       * @return {boolean} whether drill down was executed
       */

    }, {
      key: "drillDown",
      value: function drillDown(element) {
        var parent = this._injector.get('_parent', false); // no parent; skip drill down


        if (!parent) {
          return false;
        }

        var view = parent.getView(element.businessObject); // no view to drill down to

        if (!view) {
          return false;
        }

        parent.open(view);
        return true;
      }
    }]);

    return DrillDown;
  }();
  DrillDown.$inject = ['injector', 'eventBus', 'overlays', 'config.drillDown'];

  var DrillDownModule = {
    __depends__: [OverlaysModule],
    __init__: ['drillDown'],
    drillDown: ['type', DrillDown]
  };

  /**
   * This file must not be changed or exchanged.
   *
   * @see http://bpmn.io/license for more information.
   */
  // eslint-disable-next-line

  var BPMNIO_LOGO_SVG = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14.02 5.57" width="53" height="21" style="vertical-align:middle"><path fill="#000000" d="M1.88.92v.14c0 .41-.13.68-.4.8.33.14.46.44.46.86v.33c0 .61-.33.95-.95.95H0V0h.95c.65 0 .93.3.93.92zM.63.57v1.06h.24c.24 0 .38-.1.38-.43V.98c0-.28-.1-.4-.32-.4zm0 1.63v1.22h.36c.2 0 .32-.1.32-.39v-.35c0-.37-.12-.48-.4-.48H.63zM4.18.99v.52c0 .64-.31.98-.94.98h-.3V4h-.62V0h.92c.63 0 .94.35.94.99zM2.94.57v1.35h.3c.2 0 .3-.09.3-.37v-.6c0-.29-.1-.38-.3-.38h-.3zm2.89 2.27L6.25 0h.88v4h-.6V1.12L6.1 3.99h-.6l-.46-2.82v2.82h-.55V0h.87zM8.14 1.1V4h-.56V0h.79L9 2.4V0h.56v4h-.64zm2.49 2.29v.6h-.6v-.6zM12.12 1c0-.63.33-1 .95-1 .61 0 .95.37.95 1v2.04c0 .64-.34 1-.95 1-.62 0-.95-.37-.95-1zm.62 2.08c0 .28.13.39.33.39s.32-.1.32-.4V.98c0-.29-.12-.4-.32-.4s-.33.11-.33.4z"/><path fill="#000000" d="M0 4.53h14.02v1.04H0zM11.08 0h.63v.62h-.63zm.63 4V1h-.63v2.98z"/></svg>';
  var BPMNIO_IMG = BPMNIO_LOGO_SVG;

  function css(attrs) {
    return attrs.join(';');
  }

  var LIGHTBOX_STYLES = css(['z-index: 1001', 'position: fixed', 'top: 0', 'left: 0', 'right: 0', 'bottom: 0']);
  var BACKDROP_STYLES = css(['width: 100%', 'height: 100%', 'background: rgba(40,40,40,0.2)']);
  var NOTICE_STYLES = css(['position: absolute', 'left: 50%', 'top: 40%', 'transform: translate(-50%)', 'width: 260px', 'padding: 10px', 'background: white', 'box-shadow: 0 1px 4px rgba(0,0,0,0.3)', 'font-family: Helvetica, Arial, sans-serif', 'font-size: 14px', 'display: flex', 'line-height: 1.3']);
  /* eslint-disable max-len */

  var LIGHTBOX_MARKUP = '<div class="bjs-powered-by-lightbox" style="' + LIGHTBOX_STYLES + '">' + '<div class="backdrop" style="' + BACKDROP_STYLES + '"></div>' + '<div class="notice" style="' + NOTICE_STYLES + '">' + '<a href="https://bpmn.io" target="_blank" rel="noopener" style="margin: 15px 20px 15px 10px; align-self: center;' + '">' + BPMNIO_IMG + '</a>' + '<span>' + 'Web-based tooling for BPMN, DMN and CMMN diagrams ' + 'powered by <a href="https://bpmn.io" target="_blank" rel="noopener">bpmn.io</a>.' + '</span>' + '</div>' + '</div>';
  /* eslint-enable */

  var lightbox;
  function open() {
    if (!lightbox) {
      lightbox = domify(LIGHTBOX_MARKUP);
      delegate.bind(lightbox, '.backdrop', 'click', function (event) {
        document.body.removeChild(lightbox);
      });
    }

    document.body.appendChild(lightbox);
  }

  function ownKeys$1(object, enumerableOnly) {
    var keys = Object.keys(object);

    if (Object.getOwnPropertySymbols) {
      var symbols = Object.getOwnPropertySymbols(object);
      if (enumerableOnly) symbols = symbols.filter(function (sym) {
        return Object.getOwnPropertyDescriptor(object, sym).enumerable;
      });
      keys.push.apply(keys, symbols);
    }

    return keys;
  }

  function _objectSpread$1(target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i] != null ? arguments[i] : {};

      if (i % 2) {
        ownKeys$1(source, true).forEach(function (key) {
          _defineProperty$1(target, key, source[key]);
        });
      } else if (Object.getOwnPropertyDescriptors) {
        Object.defineProperties(target, Object.getOwnPropertyDescriptors(source));
      } else {
        ownKeys$1(source).forEach(function (key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
    }

    return target;
  }

  function _defineProperty$1(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  function _toConsumableArray$1(arr) {
    return _arrayWithoutHoles$1(arr) || _iterableToArray$1(arr) || _nonIterableSpread$1();
  }

  function _nonIterableSpread$1() {
    throw new TypeError("Invalid attempt to spread non-iterable instance");
  }

  function _iterableToArray$1(iter) {
    if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter);
  }

  function _arrayWithoutHoles$1(arr) {
    if (Array.isArray(arr)) {
      for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) {
        arr2[i] = arr[i];
      }

      return arr2;
    }
  }

  function _objectWithoutProperties(source, excluded) {
    if (source == null) return {};

    var target = _objectWithoutPropertiesLoose(source, excluded);

    var key, i;

    if (Object.getOwnPropertySymbols) {
      var sourceSymbolKeys = Object.getOwnPropertySymbols(source);

      for (i = 0; i < sourceSymbolKeys.length; i++) {
        key = sourceSymbolKeys[i];
        if (excluded.indexOf(key) >= 0) continue;
        if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue;
        target[key] = source[key];
      }
    }

    return target;
  }

  function _objectWithoutPropertiesLoose(source, excluded) {
    if (source == null) return {};
    var target = {};
    var sourceKeys = Object.keys(source);
    var key, i;

    for (i = 0; i < sourceKeys.length; i++) {
      key = sourceKeys[i];
      if (excluded.indexOf(key) >= 0) continue;
      target[key] = source[key];
    }

    return target;
  }
  /**
   * A viewer for DMN diagrams.
   *
   * Have a look at {@link NavigatedViewer} or {@link Modeler} for bundles that include
   * additional features.
   *
   *
   * ## Extending the Viewer
   *
   * In order to extend the viewer pass extension modules to bootstrap via the
   * `additionalModules` option. An extension module is an object that exposes
   * named services.
   *
   * The following example depicts the integration of a simple
   * logging component that integrates with interaction events:
   *
   *
   * ```javascript
   *
   * // logging component
   * function InteractionLogger(eventBus) {
   *   eventBus.on('element.hover', function(event) {
   *     console.log()
   *   })
   * }
   *
   * InteractionLogger.$inject = [ 'eventBus' ]; // minification save
   *
   * // extension module
   * var extensionModule = {
   *   __init__: [ 'interactionLogger' ],
   *   interactionLogger: [ 'type', InteractionLogger ]
   * };
   *
   * // extend the viewer
   * var drdViewer = new Viewer({ additionalModules: [ extensionModule ] });
   * drdViewer.importXML(...);
   * ```
   *
   * @param {Object} options configuration options to pass to the viewer
   * @param {DOMElement} [options.container]
   *        the container to render the viewer in, defaults to body
   * @param {Array<didi.Module>} [options.modules]
   *        a list of modules to override the default modules
   * @param {Array<didi.Module>} [options.additionalModules]
   *        a list of modules to use with the default modules
   */

  function Viewer(options) {
    this._container = this._createContainer();
    /* <project-logo> */

    addProjectLogo(this._container);
    /* </project-logo> */

    this._init(this._container, options);
  }
  inherits_browser$1(Viewer, Diagram);
  /**
   * Export the currently displayed DMN diagram as
   * an SVG image.
   *
   * @param {Object} [options]
   * @param {Function} done invoked with (err, svgStr)
   */

  Viewer.prototype.saveSVG = function (options, done) {
    if (!done) {
      done = options;
      options = {};
    }

    var canvas = this.get('canvas');
    var contentNode = canvas.getDefaultLayer(),
        defsNode = query('defs', canvas._svg);
    var contents = innerSVG(contentNode),
        defs = defsNode && defsNode.outerHTML || '';
    var bbox = contentNode.getBBox();
    /* eslint-disable max-len */

    var svg = '<?xml version="1.0" encoding="utf-8"?>\n' + '<!-- created with dmn-js / http://bpmn.io -->\n' + '<!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">\n' + '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" ' + 'width="' + bbox.width + '" height="' + bbox.height + '" ' + 'viewBox="' + bbox.x + ' ' + bbox.y + ' ' + bbox.width + ' ' + bbox.height + '" version="1.1">' + defs + contents + '</svg>';
    /* eslint-enable */

    done(null, svg);
  };

  Viewer.prototype.getModules = function () {
    return this._modules;
  };
  /**
   * Destroy the viewer instance and remove all its
   * remainders from the document tree.
   */


  Viewer.prototype.destroy = function () {
    // diagram destroy
    Diagram.prototype.destroy.call(this); // dom detach

    remove(this._container);
  };
  /**
   * Register an event listener
   *
   * Remove a previously added listener via {@link #off(event, callback)}.
   *
   * @param {string} event
   * @param {number} [priority]
   * @param {Function} callback
   * @param {Object} [that]
   */


  Viewer.prototype.on = function (event, priority, callback, target) {
    return this.get('eventBus').on(event, priority, callback, target);
  };
  /**
   * De-register an event listener
   *
   * @param {string} event
   * @param {Function} callback
   */


  Viewer.prototype.off = function (event, callback) {
    this.get('eventBus').off(event, callback);
  };

  Viewer.prototype._init = function (container, options) {
    var additionalModules = options.additionalModules,
        canvas = options.canvas,
        additionalOptions = _objectWithoutProperties(options, ["additionalModules", "canvas"]);

    var baseModules = options.modules || this.getModules(),
        staticModules = [{
      drd: ['value', this]
    }];
    var modules = [].concat(staticModules, _toConsumableArray$1(baseModules), _toConsumableArray$1(additionalModules || []));

    var diagramOptions = _objectSpread$1({}, additionalOptions, {
      canvas: _objectSpread$1({}, canvas, {
        container: container
      }),
      modules: modules
    }); // invoke diagram constructor


    Diagram.call(this, diagramOptions);

    if (options && options.container) {
      this.attachTo(options.container);
    }
  };
  /**
   * Emit an event on the underlying {@link EventBus}
   *
   * @param  {string} type
   * @param  {Object} event
   *
   * @return {Object} event processing result (if any)
   */


  Viewer.prototype._emit = function (type, event) {
    return this.get('eventBus').fire(type, event);
  };

  Viewer.prototype._createContainer = function () {
    return domify('<div class="dmn-drd-container"></div>');
  };

  Viewer.prototype.open = function (definitions, done) {
    var err; // use try/catch to not swallow synchronous exceptions
    // that may be raised during model parsing

    try {
      if (this._definitions) {
        // clear existing rendered diagram
        this.clear();
      } // update definitions


      this._definitions = definitions; // perform graphical import

      return importDRD(this, definitions, done);
    } catch (e) {
      err = e;
    }

    return done(err);
  };
  /**
   * Attach viewer to given parent node.
   *
   * @param  {Element} parentNode
   */


  Viewer.prototype.attachTo = function (parentNode) {
    if (!parentNode) {
      throw new Error('parentNode required');
    } // ensure we detach from the
    // previous, old parent


    this.detach();
    var container = this._container;
    parentNode.appendChild(container);

    this._emit('attach', {});

    this.get('canvas').resized();
  };
  /**
   * Detach viewer from parent node, if attached.
   */


  Viewer.prototype.detach = function () {
    var container = this._container,
        parentNode = container.parentNode;

    if (!parentNode) {
      return;
    }

    this._emit('detach', {});

    parentNode.removeChild(container);
  };
  Viewer.prototype._modules = [CoreModule$1, TranslateModule, SelectionModule, OverlaysModule, DefinitionPropertiesModule, DrillDownModule];
  /**
   * Adds the project logo to the diagram container as
   * required by the bpmn.io license.
   *
   * @see http://bpmn.io/license
   *
   * @param {Element} container
   */

  function addProjectLogo(container) {
    var linkMarkup = '<a href="http://bpmn.io" ' + 'target="_blank" ' + 'class="bjs-powered-by" ' + 'title="Powered by bpmn.io" ' + 'style="position: absolute; bottom: 15px; right: 15px; z-index: 100;">' + BPMNIO_IMG + '</a>';
    var linkElement = domify(linkMarkup);
    container.appendChild(linkElement);
    componentEvent.bind(linkElement, 'click', function (event) {
      open();
      event.preventDefault();
    });
  }
  /* </project-logo> */

  function _typeof$2(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$2 = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$2 = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$2(obj);
  }

  function _possibleConstructorReturn$1(self, call) {
    if (call && (_typeof$2(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$1(self);
  }

  function _getPrototypeOf$1(o) {
    _getPrototypeOf$1 = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$1(o);
  }

  function _assertThisInitialized$1(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$1(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$1(subClass, superClass);
  }

  function _setPrototypeOf$1(o, p) {
    _setPrototypeOf$1 = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$1(o, p);
  }

  function _classCallCheck$3(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }
  var Base$2 = function Base(attrs) {
    _classCallCheck$3(this, Base);

    assign(this, attrs);
    /**
     * The object that backs up the shape
     *
     * @name Base#businessObject
     * @type Object
     */

    defineProperty$2(this, 'businessObject', {
      writable: true
    });
  };
  var Root$1 =
  /*#__PURE__*/
  function (_Base) {
    _inherits$1(Root, _Base);

    function Root(attrs) {
      var _this;

      _classCallCheck$3(this, Root);

      _this = _possibleConstructorReturn$1(this, _getPrototypeOf$1(Root).call(this, attrs));
      /**
       * The tables rows
       *
       * @name Root#rows
       * @type Row
       */

      defineProperty$2(_assertThisInitialized$1(_this), 'rows', {
        enumerable: true,
        value: _this.rows || []
      });
      /**
       * The tables columns
       *
       * @name Root#cols
       * @type Col
       */

      defineProperty$2(_assertThisInitialized$1(_this), 'cols', {
        enumerable: true,
        value: _this.cols || []
      });
      return _this;
    }

    return Root;
  }(Base$2);
  var Row =
  /*#__PURE__*/
  function (_Base2) {
    _inherits$1(Row, _Base2);

    function Row(attrs) {
      var _this2;

      _classCallCheck$3(this, Row);

      _this2 = _possibleConstructorReturn$1(this, _getPrototypeOf$1(Row).call(this, attrs));
      /**
       * Reference to the table
       *
       * @name Row#root
       * @type Root
       */

      defineProperty$2(_assertThisInitialized$1(_this2), 'root', {
        writable: true
      });
      /**
       * Reference to contained cells
       *
       * @name Row#cells
       * @type Cell
       */

      defineProperty$2(_assertThisInitialized$1(_this2), 'cells', {
        enumerable: true,
        value: _this2.cells || []
      });
      return _this2;
    }

    return Row;
  }(Base$2);
  var Col =
  /*#__PURE__*/
  function (_Base3) {
    _inherits$1(Col, _Base3);

    function Col(attrs) {
      var _this3;

      _classCallCheck$3(this, Col);

      _this3 = _possibleConstructorReturn$1(this, _getPrototypeOf$1(Col).call(this, attrs));
      /**
       * Reference to the table
       *
       * @name Col#table
       * @type Root
       */

      defineProperty$2(_assertThisInitialized$1(_this3), 'root', {
        writable: true
      });
      /**
       * Reference to contained cells
       *
       * @name Row#cells
       * @type Cell
       */

      defineProperty$2(_assertThisInitialized$1(_this3), 'cells', {
        enumerable: true,
        value: _this3.cells || []
      });
      return _this3;
    }

    return Col;
  }(Base$2);
  var Cell =
  /*#__PURE__*/
  function (_Base4) {
    _inherits$1(Cell, _Base4);

    function Cell(attrs) {
      var _this4;

      _classCallCheck$3(this, Cell);

      _this4 = _possibleConstructorReturn$1(this, _getPrototypeOf$1(Cell).call(this, attrs));
      /**
       * Reference to the row
       *
       * @name Cell#row
       * @type Row
       */

      defineProperty$2(_assertThisInitialized$1(_this4), 'row', {
        writable: true
      });
      /**
       * Reference to the col
       *
       * @name Cell#col
       * @type Col
       */

      defineProperty$2(_assertThisInitialized$1(_this4), 'col', {
        writable: true
      });
      return _this4;
    }

    return Cell;
  }(Base$2);
  var TYPES = {
    root: Root$1,
    row: Row,
    col: Col,
    cell: Cell
  };
  function create$2(type, attrs) {
    var Type = TYPES[type];

    if (!Type) {
      throw new Error('unknown type ' + type);
    }

    return new Type(attrs);
  } // helpers /////////////

  function defineProperty$2(el, prop, options) {
    Object.defineProperty(el, prop, options);
  }

  function _classCallCheck$4(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$3(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$3(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$3(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$3(Constructor, staticProps);
    return Constructor;
  }

  var ElementFactory$1 =
  /*#__PURE__*/
  function () {
    function ElementFactory() {
      _classCallCheck$4(this, ElementFactory);

      this._uid = 12;
    }

    _createClass$3(ElementFactory, [{
      key: "create",
      value: function create(type) {
        var attrs = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : {};

        if (!attrs.id) {
          attrs.id = type + '_' + this._uid++;
        }

        return create$2(type, attrs);
      }
    }, {
      key: "createRoot",
      value: function createRoot(attrs) {
        return this.create('root', attrs);
      }
    }, {
      key: "createRow",
      value: function createRow(attrs) {
        return this.create('row', attrs);
      }
    }, {
      key: "createCol",
      value: function createCol(attrs) {
        return this.create('col', attrs);
      }
    }, {
      key: "createCell",
      value: function createCell(attrs) {
        return this.create('cell', attrs);
      }
    }]);

    return ElementFactory;
  }();

  function _classCallCheck$5(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$4(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$4(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$4(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$4(Constructor, staticProps);
    return Constructor;
  }

  var ElementRegistry$1 =
  /*#__PURE__*/
  function () {
    function ElementRegistry(eventBus) {
      _classCallCheck$5(this, ElementRegistry);

      this._eventBus = eventBus;
      this._elements = {};
      eventBus.on('table.clear', this.clear.bind(this));
    }

    _createClass$4(ElementRegistry, [{
      key: "add",
      value: function add(element, type) {
        var id = element.id;
        this._elements[id] = element;
      }
    }, {
      key: "remove",
      value: function remove(element) {
        var id = element.id || element;
        delete this._elements[id];
      }
    }, {
      key: "get",
      value: function get(id) {
        return this._elements[id];
      }
    }, {
      key: "getAll",
      value: function getAll() {
        return values(this._elements);
      }
    }, {
      key: "forEach",
      value: function forEach(fn) {
        values(this._elements).forEach(function (element) {
          return fn(element);
        });
      }
    }, {
      key: "filter",
      value: function filter(fn) {
        return values(this._elements).filter(function (element) {
          return fn(element);
        });
      }
    }, {
      key: "clear",
      value: function clear() {
        this._elements = {};
      }
    }, {
      key: "updateId",
      value: function updateId(element, newId) {
        this._validateId(newId);

        if (typeof element === 'string') {
          element = this.get(element);
        }

        this._eventBus.fire('element.updateId', {
          element: element,
          newId: newId
        });

        this.remove(element);
        element.id = newId;
        this.add(element);
      }
      /**
      * Validate the suitability of the given id and signals a problem
      * with an exception.
      *
      * @param {String} id
      *
      * @throws {Error} if id is empty or already assigned
      */

    }, {
      key: "_validateId",
      value: function _validateId(id) {
        if (!id) {
          throw new Error('element must have an id');
        }

        if (this._elements[id]) {
          throw new Error('element with id ' + id + ' already added');
        }
      }
    }]);

    return ElementRegistry;
  }();
  ElementRegistry$1.$inject = ['eventBus']; // helpers

  function values(obj) {
    return Object.keys(obj).map(function (k) {
      return obj[k];
    });
  }

  function _classCallCheck$6(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$5(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$5(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$5(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$5(Constructor, staticProps);
    return Constructor;
  }

  var ChangeSupport =
  /*#__PURE__*/
  function () {
    function ChangeSupport(eventBus) {
      var _this = this;

      _classCallCheck$6(this, ChangeSupport);

      this._listeners = {};
      eventBus.on('elements.changed', function (_ref) {
        var elements = _ref.elements;

        _this.elementsChanged(elements);
      });
      eventBus.on('root.remove', function (context) {
        var oldRootId = context.root.id;

        if (_this._listeners[oldRootId]) {
          eventBus.once('root.add', function (context) {
            var newRootId = context.root.id;

            _this.updateId(oldRootId, newRootId);
          });
        }
      });
      eventBus.on('element.updateId', function (_ref2) {
        var element = _ref2.element,
            newId = _ref2.newId;

        _this.updateId(element.id, newId);
      });
    }

    _createClass$5(ChangeSupport, [{
      key: "elementsChanged",
      value: function elementsChanged(elements) {
        var invoked = {};
        var elementsLength = elements.length;

        for (var i = 0; i < elementsLength; i++) {
          var id = elements[i].id;

          if (invoked[id]) {
            return;
          }

          invoked[id] = true;
          var listenersLength = this._listeners[id] && this._listeners[id].length;

          if (listenersLength) {
            for (var j = 0; j < listenersLength; j++) {
              // listeners might remove themselves before they get called
              this._listeners[id][j] && this._listeners[id][j]();
            }
          }
        }
      }
    }, {
      key: "onElementsChanged",
      value: function onElementsChanged(id, listener) {
        if (!this._listeners[id]) {
          this._listeners[id] = [];
        } // avoid push for better performance


        this._listeners[id][this._listeners[id].length] = listener;
      }
    }, {
      key: "offElementsChanged",
      value: function offElementsChanged(id, listener) {
        if (!this._listeners[id]) {
          return;
        }

        if (listener) {
          var idx = this._listeners[id].indexOf(listener);

          if (idx !== -1) {
            this._listeners[id].splice(idx, 1);
          }
        } else {
          this._listeners[id].length = 0;
        }
      }
    }, {
      key: "updateId",
      value: function updateId(oldId, newId) {
        if (this._listeners[oldId]) {
          this._listeners[newId] = this._listeners[oldId];
          delete this._listeners[oldId];
        }
      }
    }]);

    return ChangeSupport;
  }();
  ChangeSupport.$inject = ['eventBus'];

  function _classCallCheck$7(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$6(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$6(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$6(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$6(Constructor, staticProps);
    return Constructor;
  }
  var DEFAULT_PRIORITY$1 = 1000;

  var Components =
  /*#__PURE__*/
  function () {
    function Components() {
      _classCallCheck$7(this, Components);

      this._listeners = {};
    }

    _createClass$6(Components, [{
      key: "getComponent",
      value: function getComponent(type, context) {
        var listeners = this._listeners[type];

        if (!listeners) {
          return;
        }

        var component;

        for (var i = 0; i < listeners.length; i++) {
          component = listeners[i].callback(context);

          if (component) {
            break;
          }
        }

        return component;
      }
    }, {
      key: "getComponents",
      value: function getComponents(type, context) {
        var listeners = this._listeners[type];
        var components = [];

        if (!listeners) {
          return components;
        }

        for (var i = 0; i < listeners.length; i++) {
          var component = listeners[i].callback(context);

          if (component) {
            components.push(component);
          }
        }

        if (!components.length) {
          return components;
        }

        return components;
      }
    }, {
      key: "onGetComponent",
      value: function onGetComponent(type, priority, callback) {
        if (isFunction(priority)) {
          callback = priority;
          priority = DEFAULT_PRIORITY$1;
        }

        if (!isNumber(priority)) {
          throw new Error('priority must be a number');
        }

        var listeners = this._getListeners(type);

        var existingListener, idx;
        var newListener = {
          priority: priority,
          callback: callback
        };

        for (idx = 0; existingListener = listeners[idx]; idx++) {
          if (existingListener.priority < priority) {
            // prepend newListener at before existingListener
            listeners.splice(idx, 0, newListener);
            return;
          }
        }

        listeners.push(newListener);
      }
    }, {
      key: "offGetComponent",
      value: function offGetComponent(type, callback) {
        var listeners = this._getListeners(type);

        var listener, listenerCallback, idx;

        if (callback) {
          // move through listeners from back to front
          // and remove matching listeners
          for (idx = listeners.length - 1; listener = listeners[idx]; idx--) {
            listenerCallback = listener.callback;

            if (listenerCallback === callback) {
              listeners.splice(idx, 1);
            }
          }
        } else {
          // clear listeners
          listeners.length = 0;
        }
      }
    }, {
      key: "_getListeners",
      value: function _getListeners(type) {
        var listeners = this._listeners[type];

        if (!listeners) {
          this._listeners[type] = listeners = [];
        }

        return listeners;
      }
    }]);

    return Components;
  }();

  var NO_OP = '$NO_OP';
  var ERROR_MSG = 'a runtime error occured! Use Inferno in development environment to find the error.';
  var isBrowser = !!(typeof window !== 'undefined' && window.document);
  var isArray$2 = Array.isArray;

  function isStringOrNumber(o) {
    var type = _typeof(o);

    return type === 'string' || type === 'number';
  }

  function isNullOrUndef(o) {
    return isUndefined$2(o) || isNull(o);
  }

  function isInvalid(o) {
    return isNull(o) || o === false || isTrue(o) || isUndefined$2(o);
  }

  function isFunction$1(o) {
    return typeof o === 'function';
  }

  function isString$1(o) {
    return typeof o === 'string';
  }

  function isNumber$1(o) {
    return typeof o === 'number';
  }

  function isNull(o) {
    return o === null;
  }

  function isTrue(o) {
    return o === true;
  }

  function isUndefined$2(o) {
    return o === void 0;
  }

  function isObject$1(o) {
    return _typeof(o) === 'object';
  }

  function throwError(message) {
    if (!message) {
      message = ERROR_MSG;
    }

    throw new Error("Inferno Error: " + message);
  }

  function warning(message) {
    // tslint:disable-next-line:no-console
    console.error(message);
  }

  function combineFrom(first, second) {
    var out = {};

    if (first) {
      for (var key in first) {
        out[key] = first[key];
      }
    }

    if (second) {
      for (var key$1 in second) {
        out[key$1] = second[key$1];
      }
    }

    return out;
  }

  function getTagName(input) {
    var tagName;

    if (isArray$2(input)) {
      var arrayText = input.length > 3 ? input.slice(0, 3).toString() + ',...' : input.toString();
      tagName = 'Array(' + arrayText + ')';
    } else if (isStringOrNumber(input)) {
      tagName = 'Text(' + input + ')';
    } else if (isInvalid(input)) {
      tagName = 'InvalidVNode(' + input + ')';
    } else {
      var flags = input.flags;

      if (flags & 481
      /* Element */
      ) {
          tagName = "<" + input.type + (input.className ? ' class="' + input.className + '"' : '') + ">";
        } else if (flags & 16
      /* Text */
      ) {
          tagName = "Text(" + input.children + ")";
        } else if (flags & 1024
      /* Portal */
      ) {
          tagName = "Portal*";
        } else {
        var type = input.type; // Fallback for IE

        var componentName = type.name || type.displayName || type.constructor.name || (type.toString().match(/^function\s*([^\s(]+)/) || [])[1];
        tagName = "<" + componentName + " />";
      }
    }

    return '>> ' + tagName + '\n';
  }

  function DEV_ValidateKeys(vNodeTree, forceKeyed) {
    var foundKeys = {};

    for (var i = 0, len = vNodeTree.length; i < len; i++) {
      var childNode = vNodeTree[i];

      if (isArray$2(childNode)) {
        return 'Encountered ARRAY in mount, array must be flattened, or normalize used. Location: \n' + getTagName(childNode);
      }

      if (isInvalid(childNode)) {
        if (forceKeyed) {
          return 'Encountered invalid node when preparing to keyed algorithm. Location: \n' + getTagName(childNode);
        } else if (Object.keys(foundKeys).length !== 0) {
          return 'Encountered invalid node with mixed keys. Location: \n' + getTagName(childNode);
        }

        continue;
      }

      if (_typeof(childNode) === 'object') {
        childNode.isValidated = true;
      } // Key can be undefined, null too. But typescript complains for no real reason


      var key = childNode.key;

      if (!isNullOrUndef(key) && !isStringOrNumber(key)) {
        return 'Encountered child vNode where key property is not string or number. Location: \n' + getTagName(childNode);
      }

      var children = childNode.children;
      var childFlags = childNode.childFlags;

      if (!isInvalid(children)) {
        var val = void 0;

        if (childFlags & 12
        /* MultipleChildren */
        ) {
            val = DEV_ValidateKeys(children, childNode.childFlags & 8
            /* HasKeyedChildren */
            );
          } else if (childFlags === 2
        /* HasVNodeChildren */
        ) {
            val = DEV_ValidateKeys([children], childNode.childFlags & 8
            /* HasKeyedChildren */
            );
          }

        if (val) {
          val += getTagName(childNode);
          return val;
        }
      }

      if (forceKeyed && isNullOrUndef(key)) {
        return 'Encountered child without key during keyed algorithm. If this error points to Array make sure children is flat list. Location: \n' + getTagName(childNode);
      } else if (!forceKeyed && isNullOrUndef(key)) {
        if (Object.keys(foundKeys).length !== 0) {
          return 'Encountered children with key missing. Location: \n' + getTagName(childNode);
        }

        continue;
      }

      if (foundKeys[key]) {
        return 'Encountered two children with same key: {' + key + '}. Location: \n' + getTagName(childNode);
      }

      foundKeys[key] = true;
    }
  }

  function validateVNodeElementChildren(vNode) {
    {
      if (vNode.childFlags & 1
      /* HasInvalidChildren */
      ) {
          return;
        }

      if (vNode.flags & 64
      /* InputElement */
      ) {
          throwError("input elements can't have children.");
        }

      if (vNode.flags & 128
      /* TextareaElement */
      ) {
          throwError("textarea elements can't have children.");
        }

      if (vNode.flags & 481
      /* Element */
      ) {
          var voidTypes = ['area', 'base', 'br', 'col', 'command', 'embed', 'hr', 'img', 'input', 'keygen', 'link', 'meta', 'param', 'source', 'track', 'wbr'];
          var tag = vNode.type.toLowerCase();

          if (tag === 'media') {
            throwError("media elements can't have children.");
          }

          var idx = voidTypes.indexOf(tag);

          if (idx !== -1) {
            throwError(voidTypes[idx] + " elements can't have children.");
          }
        }
    }
  }

  function validateKeys(vNode) {
    {
      // Checks if there is any key missing or duplicate keys
      if (vNode.isValidated === false && vNode.children && vNode.flags & 481
      /* Element */
      ) {
          var error = DEV_ValidateKeys(Array.isArray(vNode.children) ? vNode.children : [vNode.children], (vNode.childFlags & 8
          /* HasKeyedChildren */
          ) > 0);

          if (error) {
            throwError(error + getTagName(vNode));
          }
        }

      vNode.isValidated = true;
    }
  }

  var keyPrefix = '$';

  function getVNode(childFlags, children, className, flags, key, props, ref, type) {
    {
      return {
        childFlags: childFlags,
        children: children,
        className: className,
        dom: null,
        flags: flags,
        isValidated: false,
        key: key === void 0 ? null : key,
        parentVNode: null,
        props: props === void 0 ? null : props,
        ref: ref === void 0 ? null : ref,
        type: type
      };
    }
  }

  function createVNode(flags, type, className, children, childFlags, props, key, ref) {
    {
      if (flags & 14
      /* Component */
      ) {
          throwError('Creating Component vNodes using createVNode is not allowed. Use Inferno.createComponentVNode method.');
        }
    }
    var childFlag = childFlags === void 0 ? 1
    /* HasInvalidChildren */
    : childFlags;
    var vNode = getVNode(childFlag, children, className, flags, key, props, ref, type);
    var optsVNode = options.createVNode;

    if (typeof optsVNode === 'function') {
      optsVNode(vNode);
    }

    if (childFlag === 0
    /* UnknownChildren */
    ) {
        normalizeChildren(vNode, vNode.children);
      }

    {
      validateVNodeElementChildren(vNode);
    }
    return vNode;
  }

  function createComponentVNode(flags, type, props, key, ref) {
    {
      if (flags & 1
      /* HtmlElement */
      ) {
          throwError('Creating element vNodes using createComponentVNode is not allowed. Use Inferno.createVNode method.');
        }
    }

    if ((flags & 2
    /* ComponentUnknown */
    ) > 0) {
      flags = type.prototype && isFunction$1(type.prototype.render) ? 4
      /* ComponentClass */
      : 8
      /* ComponentFunction */
      ;
    } // set default props


    var defaultProps = type.defaultProps;

    if (!isNullOrUndef(defaultProps)) {
      if (!props) {
        props = {}; // Props can be referenced and modified at application level so always create new object
      }

      for (var prop in defaultProps) {
        if (isUndefined$2(props[prop])) {
          props[prop] = defaultProps[prop];
        }
      }
    }

    if ((flags & 8
    /* ComponentFunction */
    ) > 0) {
      var defaultHooks = type.defaultHooks;

      if (!isNullOrUndef(defaultHooks)) {
        if (!ref) {
          // As ref cannot be referenced from application level, we can use the same refs object
          ref = defaultHooks;
        } else {
          for (var prop$1 in defaultHooks) {
            if (isUndefined$2(ref[prop$1])) {
              ref[prop$1] = defaultHooks[prop$1];
            }
          }
        }
      }
    }

    var vNode = getVNode(1
    /* HasInvalidChildren */
    , null, null, flags, key, props, ref, type);
    var optsVNode = options.createVNode;

    if (isFunction$1(optsVNode)) {
      optsVNode(vNode);
    }

    return vNode;
  }

  function createTextVNode(text, key) {
    return getVNode(1
    /* HasInvalidChildren */
    , isNullOrUndef(text) ? '' : text, null, 16
    /* Text */
    , key, null, null, null);
  }

  function normalizeProps(vNode) {
    var props = vNode.props;

    if (props) {
      var flags = vNode.flags;

      if (flags & 481
      /* Element */
      ) {
          if (props.children !== void 0 && isNullOrUndef(vNode.children)) {
            normalizeChildren(vNode, props.children);
          }

          if (props.className !== void 0) {
            vNode.className = props.className || null;
            props.className = undefined;
          }
        }

      if (props.key !== void 0) {
        vNode.key = props.key;
        props.key = undefined;
      }

      if (props.ref !== void 0) {
        if (flags & 8
        /* ComponentFunction */
        ) {
            vNode.ref = combineFrom(vNode.ref, props.ref);
          } else {
          vNode.ref = props.ref;
        }

        props.ref = undefined;
      }
    }

    return vNode;
  }

  function directClone(vNodeToClone) {
    var newVNode;
    var flags = vNodeToClone.flags;

    if (flags & 14
    /* Component */
    ) {
        var props;
        var propsToClone = vNodeToClone.props;

        if (!isNull(propsToClone)) {
          props = {};

          for (var key in propsToClone) {
            props[key] = propsToClone[key];
          }
        }

        newVNode = createComponentVNode(flags, vNodeToClone.type, props, vNodeToClone.key, vNodeToClone.ref);
      } else if (flags & 481
    /* Element */
    ) {
        newVNode = createVNode(flags, vNodeToClone.type, vNodeToClone.className, vNodeToClone.children, vNodeToClone.childFlags, vNodeToClone.props, vNodeToClone.key, vNodeToClone.ref);
      } else if (flags & 16
    /* Text */
    ) {
        newVNode = createTextVNode(vNodeToClone.children, vNodeToClone.key);
      } else if (flags & 1024
    /* Portal */
    ) {
        newVNode = vNodeToClone;
      }

    return newVNode;
  }

  function createVoidVNode() {
    return createTextVNode('', null);
  }

  function _normalizeVNodes(nodes, result, index, currentKey) {
    for (var len = nodes.length; index < len; index++) {
      var n = nodes[index];

      if (!isInvalid(n)) {
        var newKey = currentKey + keyPrefix + index;

        if (isArray$2(n)) {
          _normalizeVNodes(n, result, 0, newKey);
        } else {
          if (isStringOrNumber(n)) {
            n = createTextVNode(n, newKey);
          } else {
            var oldKey = n.key;
            var isPrefixedKey = isString$1(oldKey) && oldKey[0] === keyPrefix;

            if (!isNull(n.dom) || isPrefixedKey) {
              n = directClone(n);
            }

            if (isNull(oldKey) || isPrefixedKey) {
              n.key = newKey;
            } else {
              n.key = currentKey + oldKey;
            }
          }

          result.push(n);
        }
      }
    }
  }

  function normalizeChildren(vNode, children) {
    var newChildren;
    var newChildFlags = 1
    /* HasInvalidChildren */
    ; // Don't change children to match strict equal (===) true in patching

    if (isInvalid(children)) {
      newChildren = children;
    } else if (isString$1(children)) {
      newChildFlags = 2
      /* HasVNodeChildren */
      ;
      newChildren = createTextVNode(children);
    } else if (isNumber$1(children)) {
      newChildFlags = 2
      /* HasVNodeChildren */
      ;
      newChildren = createTextVNode(children + '');
    } else if (isArray$2(children)) {
      var len = children.length;

      if (len === 0) {
        newChildren = null;
        newChildFlags = 1
        /* HasInvalidChildren */
        ;
      } else {
        // we assign $ which basically means we've flagged this array for future note
        // if it comes back again, we need to clone it, as people are using it
        // in an immutable way
        // tslint:disable-next-line
        if (Object.isFrozen(children) || children['$'] === true) {
          children = children.slice();
        }

        newChildFlags = 8
        /* HasKeyedChildren */
        ;

        for (var i = 0; i < len; i++) {
          var n = children[i];

          if (isInvalid(n) || isArray$2(n)) {
            newChildren = newChildren || children.slice(0, i);

            _normalizeVNodes(children, newChildren, i, '');

            break;
          } else if (isStringOrNumber(n)) {
            newChildren = newChildren || children.slice(0, i);
            newChildren.push(createTextVNode(n, keyPrefix + i));
          } else {
            var key = n.key;
            var isNullDom = isNull(n.dom);
            var isNullKey = isNull(key);
            var isPrefixed = !isNullKey && isString$1(key) && key[0] === keyPrefix;

            if (!isNullDom || isNullKey || isPrefixed) {
              newChildren = newChildren || children.slice(0, i);

              if (!isNullDom || isPrefixed) {
                n = directClone(n);
              }

              if (isNullKey || isPrefixed) {
                n.key = keyPrefix + i;
              }

              newChildren.push(n);
            } else if (newChildren) {
              newChildren.push(n);
            }
          }
        }

        newChildren = newChildren || children;
        newChildren.$ = true;
      }
    } else {
      newChildren = children;

      if (!isNull(children.dom)) {
        newChildren = directClone(children);
      }

      newChildFlags = 2
      /* HasVNodeChildren */
      ;
    }

    vNode.children = newChildren;
    vNode.childFlags = newChildFlags;
    {
      validateVNodeElementChildren(vNode);
    }
    return vNode;
  }

  var options = {
    afterRender: null,
    beforeRender: null,
    createVNode: null,
    renderComplete: null
  };

  var xlinkNS = 'http://www.w3.org/1999/xlink';
  var xmlNS = 'http://www.w3.org/XML/1998/namespace';
  var svgNS = 'http://www.w3.org/2000/svg';
  var namespaces = {
    'xlink:actuate': xlinkNS,
    'xlink:arcrole': xlinkNS,
    'xlink:href': xlinkNS,
    'xlink:role': xlinkNS,
    'xlink:show': xlinkNS,
    'xlink:title': xlinkNS,
    'xlink:type': xlinkNS,
    'xml:base': xmlNS,
    'xml:lang': xmlNS,
    'xml:space': xmlNS
  }; // We need EMPTY_OBJ defined in one place.
  // Its used for comparison so we cant inline it into shared

  var EMPTY_OBJ = {};
  var LIFECYCLE = [];
  {
    Object.freeze(EMPTY_OBJ);
  }

  function appendChild(parentDom, dom) {
    parentDom.appendChild(dom);
  }

  function insertOrAppend(parentDom, newNode, nextNode) {
    if (isNullOrUndef(nextNode)) {
      appendChild(parentDom, newNode);
    } else {
      parentDom.insertBefore(newNode, nextNode);
    }
  }

  function documentCreateElement(tag, isSVG) {
    if (isSVG) {
      return document.createElementNS(svgNS, tag);
    }

    return document.createElement(tag);
  }

  function replaceChild(parentDom, newDom, lastDom) {
    parentDom.replaceChild(newDom, lastDom);
  }

  function removeChild(parentDom, dom) {
    parentDom.removeChild(dom);
  }

  function callAll(arrayFn) {
    var listener;

    while ((listener = arrayFn.shift()) !== undefined) {
      listener();
    }
  }

  var attachedEventCounts = {};
  var attachedEvents = {};

  function handleEvent(name, nextEvent, dom) {
    var eventsLeft = attachedEventCounts[name];
    var eventsObject = dom.$EV;

    if (nextEvent) {
      if (!eventsLeft) {
        attachedEvents[name] = attachEventToDocument(name);
        attachedEventCounts[name] = 0;
      }

      if (!eventsObject) {
        eventsObject = dom.$EV = {};
      }

      if (!eventsObject[name]) {
        attachedEventCounts[name]++;
      }

      eventsObject[name] = nextEvent;
    } else if (eventsObject && eventsObject[name]) {
      attachedEventCounts[name]--;

      if (eventsLeft === 1) {
        document.removeEventListener(normalizeEventName(name), attachedEvents[name]);
        attachedEvents[name] = null;
      }

      eventsObject[name] = nextEvent;
    }
  }

  function dispatchEvents(event, target, isClick, name, eventData) {
    var dom = target;

    while (!isNull(dom)) {
      // Html Nodes can be nested fe: span inside button in that scenario browser does not handle disabled attribute on parent,
      // because the event listener is on document.body
      // Don't process clicks on disabled elements
      if (isClick && dom.disabled) {
        return;
      }

      var eventsObject = dom.$EV;

      if (eventsObject) {
        var currentEvent = eventsObject[name];

        if (currentEvent) {
          // linkEvent object
          eventData.dom = dom;

          if (currentEvent.event) {
            currentEvent.event(currentEvent.data, event);
          } else {
            currentEvent(event);
          }

          if (event.cancelBubble) {
            return;
          }
        }
      }

      dom = dom.parentNode;
    }
  }

  function normalizeEventName(name) {
    return name.substr(2).toLowerCase();
  }

  function stopPropagation() {
    this.cancelBubble = true;

    if (!this.immediatePropagationStopped) {
      this.stopImmediatePropagation();
    }
  }

  function attachEventToDocument(name) {
    var docEvent = function docEvent(event) {
      var type = event.type;
      var isClick = type === 'click' || type === 'dblclick';

      if (isClick && event.button !== 0) {
        // Firefox incorrectly triggers click event for mid/right mouse buttons.
        // This bug has been active for 12 years.
        // https://bugzilla.mozilla.org/show_bug.cgi?id=184051
        event.stopPropagation();
        return false;
      }

      event.stopPropagation = stopPropagation; // Event data needs to be object to save reference to currentTarget getter

      var eventData = {
        dom: document
      };
      Object.defineProperty(event, 'currentTarget', {
        configurable: true,
        get: function get() {
          return eventData.dom;
        }
      });
      dispatchEvents(event, event.target, isClick, name, eventData);
      return;
    };

    document.addEventListener(normalizeEventName(name), docEvent);
    return docEvent;
  }

  function isSameInnerHTML(dom, innerHTML) {
    var tempdom = document.createElement('i');
    tempdom.innerHTML = innerHTML;
    return tempdom.innerHTML === dom.innerHTML;
  }

  function isSamePropsInnerHTML(dom, props) {
    return Boolean(props && props.dangerouslySetInnerHTML && props.dangerouslySetInnerHTML.__html && isSameInnerHTML(dom, props.dangerouslySetInnerHTML.__html));
  }

  function triggerEventListener(props, methodName, e) {
    if (props[methodName]) {
      var listener = props[methodName];

      if (listener.event) {
        listener.event(listener.data, e);
      } else {
        listener(e);
      }
    } else {
      var nativeListenerName = methodName.toLowerCase();

      if (props[nativeListenerName]) {
        props[nativeListenerName](e);
      }
    }
  }

  function createWrappedFunction(methodName, applyValue) {
    var fnMethod = function fnMethod(e) {
      e.stopPropagation();
      var vNode = this.$V; // If vNode is gone by the time event fires, no-op

      if (!vNode) {
        return;
      }

      var props = vNode.props || EMPTY_OBJ;
      var dom = vNode.dom;

      if (isString$1(methodName)) {
        triggerEventListener(props, methodName, e);
      } else {
        for (var i = 0; i < methodName.length; i++) {
          triggerEventListener(props, methodName[i], e);
        }
      }

      if (isFunction$1(applyValue)) {
        var newVNode = this.$V;
        var newProps = newVNode.props || EMPTY_OBJ;
        applyValue(newProps, dom, false, newVNode);
      }
    };

    Object.defineProperty(fnMethod, 'wrapped', {
      configurable: false,
      enumerable: false,
      value: true,
      writable: false
    });
    return fnMethod;
  }

  function isCheckedType(type) {
    return type === 'checkbox' || type === 'radio';
  }

  var onTextInputChange = createWrappedFunction('onInput', applyValueInput);
  var wrappedOnChange = createWrappedFunction(['onClick', 'onChange'], applyValueInput);
  /* tslint:disable-next-line:no-empty */

  function emptywrapper(event) {
    event.stopPropagation();
  }

  emptywrapper.wrapped = true;

  function inputEvents(dom, nextPropsOrEmpty) {
    if (isCheckedType(nextPropsOrEmpty.type)) {
      dom.onchange = wrappedOnChange;
      dom.onclick = emptywrapper;
    } else {
      dom.oninput = onTextInputChange;
    }
  }

  function applyValueInput(nextPropsOrEmpty, dom) {
    var type = nextPropsOrEmpty.type;
    var value = nextPropsOrEmpty.value;
    var checked = nextPropsOrEmpty.checked;
    var multiple = nextPropsOrEmpty.multiple;
    var defaultValue = nextPropsOrEmpty.defaultValue;
    var hasValue = !isNullOrUndef(value);

    if (type && type !== dom.type) {
      dom.setAttribute('type', type);
    }

    if (!isNullOrUndef(multiple) && multiple !== dom.multiple) {
      dom.multiple = multiple;
    }

    if (!isNullOrUndef(defaultValue) && !hasValue) {
      dom.defaultValue = defaultValue + '';
    }

    if (isCheckedType(type)) {
      if (hasValue) {
        dom.value = value;
      }

      if (!isNullOrUndef(checked)) {
        dom.checked = checked;
      }
    } else {
      if (hasValue && dom.value !== value) {
        dom.defaultValue = value;
        dom.value = value;
      } else if (!isNullOrUndef(checked)) {
        dom.checked = checked;
      }
    }
  }

  function updateChildOptionGroup(vNode, value) {
    var type = vNode.type;

    if (type === 'optgroup') {
      var children = vNode.children;
      var childFlags = vNode.childFlags;

      if (childFlags & 12
      /* MultipleChildren */
      ) {
          for (var i = 0, len = children.length; i < len; i++) {
            updateChildOption(children[i], value);
          }
        } else if (childFlags === 2
      /* HasVNodeChildren */
      ) {
          updateChildOption(children, value);
        }
    } else {
      updateChildOption(vNode, value);
    }
  }

  function updateChildOption(vNode, value) {
    var props = vNode.props || EMPTY_OBJ;
    var dom = vNode.dom; // we do this as multiple may have changed

    dom.value = props.value;

    if (isArray$2(value) && value.indexOf(props.value) !== -1 || props.value === value) {
      dom.selected = true;
    } else if (!isNullOrUndef(value) || !isNullOrUndef(props.selected)) {
      dom.selected = props.selected || false;
    }
  }

  var onSelectChange = createWrappedFunction('onChange', applyValueSelect);

  function selectEvents(dom) {
    dom.onchange = onSelectChange;
  }

  function applyValueSelect(nextPropsOrEmpty, dom, mounting, vNode) {
    var multiplePropInBoolean = Boolean(nextPropsOrEmpty.multiple);

    if (!isNullOrUndef(nextPropsOrEmpty.multiple) && multiplePropInBoolean !== dom.multiple) {
      dom.multiple = multiplePropInBoolean;
    }

    var childFlags = vNode.childFlags;

    if ((childFlags & 1
    /* HasInvalidChildren */
    ) === 0) {
      var children = vNode.children;
      var value = nextPropsOrEmpty.value;

      if (mounting && isNullOrUndef(value)) {
        value = nextPropsOrEmpty.defaultValue;
      }

      if (childFlags & 12
      /* MultipleChildren */
      ) {
          for (var i = 0, len = children.length; i < len; i++) {
            updateChildOptionGroup(children[i], value);
          }
        } else if (childFlags === 2
      /* HasVNodeChildren */
      ) {
          updateChildOptionGroup(children, value);
        }
    }
  }

  var onTextareaInputChange = createWrappedFunction('onInput', applyValueTextArea);
  var wrappedOnChange$1 = createWrappedFunction('onChange');

  function textAreaEvents(dom, nextPropsOrEmpty) {
    dom.oninput = onTextareaInputChange;

    if (nextPropsOrEmpty.onChange) {
      dom.onchange = wrappedOnChange$1;
    }
  }

  function applyValueTextArea(nextPropsOrEmpty, dom, mounting) {
    var value = nextPropsOrEmpty.value;
    var domValue = dom.value;

    if (isNullOrUndef(value)) {
      if (mounting) {
        var defaultValue = nextPropsOrEmpty.defaultValue;

        if (!isNullOrUndef(defaultValue) && defaultValue !== domValue) {
          dom.defaultValue = defaultValue;
          dom.value = defaultValue;
        }
      }
    } else if (domValue !== value) {
      /* There is value so keep it controlled */
      dom.defaultValue = value;
      dom.value = value;
    }
  }
  /**
   * There is currently no support for switching same input between controlled and nonControlled
   * If that ever becomes a real issue, then re design controlled elements
   * Currently user must choose either controlled or non-controlled and stick with that
   */


  function processElement(flags, vNode, dom, nextPropsOrEmpty, mounting, isControlled) {
    if (flags & 64
    /* InputElement */
    ) {
        applyValueInput(nextPropsOrEmpty, dom);
      } else if (flags & 256
    /* SelectElement */
    ) {
        applyValueSelect(nextPropsOrEmpty, dom, mounting, vNode);
      } else if (flags & 128
    /* TextareaElement */
    ) {
        applyValueTextArea(nextPropsOrEmpty, dom, mounting);
      }

    if (isControlled) {
      dom.$V = vNode;
    }
  }

  function addFormElementEventHandlers(flags, dom, nextPropsOrEmpty) {
    if (flags & 64
    /* InputElement */
    ) {
        inputEvents(dom, nextPropsOrEmpty);
      } else if (flags & 256
    /* SelectElement */
    ) {
        selectEvents(dom);
      } else if (flags & 128
    /* TextareaElement */
    ) {
        textAreaEvents(dom, nextPropsOrEmpty);
      }
  }

  function isControlledFormElement(nextPropsOrEmpty) {
    return nextPropsOrEmpty.type && isCheckedType(nextPropsOrEmpty.type) ? !isNullOrUndef(nextPropsOrEmpty.checked) : !isNullOrUndef(nextPropsOrEmpty.value);
  }

  function remove$3(vNode, parentDom) {
    unmount(vNode);

    if (parentDom && vNode.dom) {
      removeChild(parentDom, vNode.dom); // Let carbage collector free memory

      vNode.dom = null;
    }
  }

  function unmount(vNode) {
    var flags = vNode.flags;

    if (flags & 481
    /* Element */
    ) {
        var ref = vNode.ref;
        var props = vNode.props;

        if (isFunction$1(ref)) {
          ref(null);
        }

        var children = vNode.children;
        var childFlags = vNode.childFlags;

        if (childFlags & 12
        /* MultipleChildren */
        ) {
            unmountAllChildren(children);
          } else if (childFlags === 2
        /* HasVNodeChildren */
        ) {
            unmount(children);
          }

        if (!isNull(props)) {
          for (var name in props) {
            switch (name) {
              case 'onClick':
              case 'onDblClick':
              case 'onFocusIn':
              case 'onFocusOut':
              case 'onKeyDown':
              case 'onKeyPress':
              case 'onKeyUp':
              case 'onMouseDown':
              case 'onMouseMove':
              case 'onMouseUp':
              case 'onSubmit':
              case 'onTouchEnd':
              case 'onTouchMove':
              case 'onTouchStart':
                handleEvent(name, null, vNode.dom);
                break;
            }
          }
        }
      } else {
      var children$1 = vNode.children; // Safe guard for crashed VNode

      if (children$1) {
        if (flags & 14
        /* Component */
        ) {
            var ref$1 = vNode.ref;

            if (flags & 4
            /* ComponentClass */
            ) {
                if (isFunction$1(children$1.componentWillUnmount)) {
                  children$1.componentWillUnmount();
                }

                if (isFunction$1(ref$1)) {
                  ref$1(null);
                }

                children$1.$UN = true;

                if (children$1.$LI) {
                  unmount(children$1.$LI);
                }
              } else {
              if (!isNullOrUndef(ref$1) && isFunction$1(ref$1.onComponentWillUnmount)) {
                ref$1.onComponentWillUnmount(vNode.dom, vNode.props || EMPTY_OBJ);
              }

              unmount(children$1);
            }
          } else if (flags & 1024
        /* Portal */
        ) {
            remove$3(children$1, vNode.type);
          }
      }
    }
  }

  function unmountAllChildren(children) {
    for (var i = 0, len = children.length; i < len; i++) {
      unmount(children[i]);
    }
  }

  function removeAllChildren(dom, children) {
    unmountAllChildren(children);
    dom.textContent = '';
  }

  function createLinkEvent(linkEvent, nextValue) {
    return function (e) {
      linkEvent(nextValue.data, e);
    };
  }

  function patchEvent(name, nextValue, dom) {
    var nameLowerCase = name.toLowerCase();

    if (!isFunction$1(nextValue) && !isNullOrUndef(nextValue)) {
      var linkEvent = nextValue.event;

      if (linkEvent && isFunction$1(linkEvent)) {
        dom[nameLowerCase] = createLinkEvent(linkEvent, nextValue);
      } else {
        // Development warning
        {
          throwError("an event on a VNode \"" + name + "\". was not a function or a valid linkEvent.");
        }
      }
    } else {
      var domEvent = dom[nameLowerCase]; // if the function is wrapped, that means it's been controlled by a wrapper

      if (!domEvent || !domEvent.wrapped) {
        dom[nameLowerCase] = nextValue;
      }
    }
  }

  function getNumberStyleValue(style, value) {
    switch (style) {
      case 'animationIterationCount':
      case 'borderImageOutset':
      case 'borderImageSlice':
      case 'borderImageWidth':
      case 'boxFlex':
      case 'boxFlexGroup':
      case 'boxOrdinalGroup':
      case 'columnCount':
      case 'fillOpacity':
      case 'flex':
      case 'flexGrow':
      case 'flexNegative':
      case 'flexOrder':
      case 'flexPositive':
      case 'flexShrink':
      case 'floodOpacity':
      case 'fontWeight':
      case 'gridColumn':
      case 'gridRow':
      case 'lineClamp':
      case 'lineHeight':
      case 'opacity':
      case 'order':
      case 'orphans':
      case 'stopOpacity':
      case 'strokeDasharray':
      case 'strokeDashoffset':
      case 'strokeMiterlimit':
      case 'strokeOpacity':
      case 'strokeWidth':
      case 'tabSize':
      case 'widows':
      case 'zIndex':
      case 'zoom':
        return value;

      default:
        return value + 'px';
    }
  } // We are assuming here that we come from patchProp routine
  // -nextAttrValue cannot be null or undefined


  function patchStyle(lastAttrValue, nextAttrValue, dom) {
    var domStyle = dom.style;
    var style;
    var value;

    if (isString$1(nextAttrValue)) {
      domStyle.cssText = nextAttrValue;
      return;
    }

    if (!isNullOrUndef(lastAttrValue) && !isString$1(lastAttrValue)) {
      for (style in nextAttrValue) {
        // do not add a hasOwnProperty check here, it affects performance
        value = nextAttrValue[style];

        if (value !== lastAttrValue[style]) {
          domStyle[style] = isNumber$1(value) ? getNumberStyleValue(style, value) : value;
        }
      }

      for (style in lastAttrValue) {
        if (isNullOrUndef(nextAttrValue[style])) {
          domStyle[style] = '';
        }
      }
    } else {
      for (style in nextAttrValue) {
        value = nextAttrValue[style];
        domStyle[style] = isNumber$1(value) ? getNumberStyleValue(style, value) : value;
      }
    }
  }

  function patchProp(prop, lastValue, nextValue, dom, isSVG, hasControlledValue, lastVNode) {
    switch (prop) {
      case 'onClick':
      case 'onDblClick':
      case 'onFocusIn':
      case 'onFocusOut':
      case 'onKeyDown':
      case 'onKeyPress':
      case 'onKeyUp':
      case 'onMouseDown':
      case 'onMouseMove':
      case 'onMouseUp':
      case 'onSubmit':
      case 'onTouchEnd':
      case 'onTouchMove':
      case 'onTouchStart':
        handleEvent(prop, nextValue, dom);
        break;

      case 'children':
      case 'childrenType':
      case 'className':
      case 'defaultValue':
      case 'key':
      case 'multiple':
      case 'ref':
        break;

      case 'autoFocus':
        dom.autofocus = !!nextValue;
        break;

      case 'allowfullscreen':
      case 'autoplay':
      case 'capture':
      case 'checked':
      case 'controls':
      case 'default':
      case 'disabled':
      case 'hidden':
      case 'indeterminate':
      case 'loop':
      case 'muted':
      case 'novalidate':
      case 'open':
      case 'readOnly':
      case 'required':
      case 'reversed':
      case 'scoped':
      case 'seamless':
      case 'selected':
        dom[prop] = !!nextValue;
        break;

      case 'defaultChecked':
      case 'value':
      case 'volume':
        if (hasControlledValue && prop === 'value') {
          return;
        }

        var value = isNullOrUndef(nextValue) ? '' : nextValue;

        if (dom[prop] !== value) {
          dom[prop] = value;
        }

        break;

      case 'dangerouslySetInnerHTML':
        var lastHtml = lastValue && lastValue.__html || '';
        var nextHtml = nextValue && nextValue.__html || '';

        if (lastHtml !== nextHtml) {
          if (!isNullOrUndef(nextHtml) && !isSameInnerHTML(dom, nextHtml)) {
            if (!isNull(lastVNode)) {
              if (lastVNode.childFlags & 12
              /* MultipleChildren */
              ) {
                  unmountAllChildren(lastVNode.children);
                } else if (lastVNode.childFlags === 2
              /* HasVNodeChildren */
              ) {
                  unmount(lastVNode.children);
                }

              lastVNode.children = null;
              lastVNode.childFlags = 1
              /* HasInvalidChildren */
              ;
            }

            dom.innerHTML = nextHtml;
          }
        }

        break;

      default:
        if (prop[0] === 'o' && prop[1] === 'n') {
          patchEvent(prop, nextValue, dom);
        } else if (isNullOrUndef(nextValue)) {
          dom.removeAttribute(prop);
        } else if (prop === 'style') {
          patchStyle(lastValue, nextValue, dom);
        } else if (isSVG && namespaces[prop]) {
          // We optimize for isSVG being false
          // If we end up in this path we can read property again
          dom.setAttributeNS(namespaces[prop], prop, nextValue);
        } else {
          dom.setAttribute(prop, nextValue);
        }

        break;
    }
  }

  function mountProps(vNode, flags, props, dom, isSVG) {
    var hasControlledValue = false;
    var isFormElement = (flags & 448
    /* FormElement */
    ) > 0;

    if (isFormElement) {
      hasControlledValue = isControlledFormElement(props);

      if (hasControlledValue) {
        addFormElementEventHandlers(flags, dom, props);
      }
    }

    for (var prop in props) {
      // do not add a hasOwnProperty check here, it affects performance
      patchProp(prop, null, props[prop], dom, isSVG, hasControlledValue, null);
    }

    if (isFormElement) {
      processElement(flags, vNode, dom, props, true, hasControlledValue);
    }
  }

  function createClassComponentInstance(vNode, Component, props, context) {
    var instance = new Component(props, context);
    vNode.children = instance;
    instance.$V = vNode;
    instance.$BS = false;
    instance.context = context;

    if (instance.props === EMPTY_OBJ) {
      instance.props = props;
    }

    instance.$UN = false;

    if (isFunction$1(instance.componentWillMount)) {
      instance.$BR = true;
      instance.componentWillMount();

      if (instance.$PSS) {
        var state = instance.state;
        var pending = instance.$PS;

        if (isNull(state)) {
          instance.state = pending;
        } else {
          for (var key in pending) {
            state[key] = pending[key];
          }
        }

        instance.$PSS = false;
        instance.$PS = null;
      }

      instance.$BR = false;
    }

    if (isFunction$1(options.beforeRender)) {
      options.beforeRender(instance);
    }

    var input = handleComponentInput(instance.render(props, instance.state, context), vNode);
    var childContext;

    if (isFunction$1(instance.getChildContext)) {
      childContext = instance.getChildContext();
    }

    if (isNullOrUndef(childContext)) {
      instance.$CX = context;
    } else {
      instance.$CX = combineFrom(context, childContext);
    }

    if (isFunction$1(options.afterRender)) {
      options.afterRender(instance);
    }

    instance.$LI = input;
    return instance;
  }

  function handleComponentInput(input, componentVNode) {
    // Development validation
    {
      if (isArray$2(input)) {
        throwError('a valid Inferno VNode (or null) must be returned from a component render. You may have returned an array or an invalid object.');
      }
    }

    if (isInvalid(input)) {
      input = createVoidVNode();
    } else if (isStringOrNumber(input)) {
      input = createTextVNode(input, null);
    } else {
      if (input.dom) {
        input = directClone(input);
      }

      if (input.flags & 14
      /* Component */
      ) {
          // if we have an input that is also a component, we run into a tricky situation
          // where the root vNode needs to always have the correct DOM entry
          // we can optimise this in the future, but this gets us out of a lot of issues
          input.parentVNode = componentVNode;
        }
    }

    return input;
  }

  function mount(vNode, parentDom, context, isSVG) {
    var flags = vNode.flags;

    if (flags & 481
    /* Element */
    ) {
        return mountElement(vNode, parentDom, context, isSVG);
      }

    if (flags & 14
    /* Component */
    ) {
        return mountComponent(vNode, parentDom, context, isSVG, (flags & 4
        /* ComponentClass */
        ) > 0);
      }

    if (flags & 512
    /* Void */
    || flags & 16
    /* Text */
    ) {
        return mountText(vNode, parentDom);
      }

    if (flags & 1024
    /* Portal */
    ) {
        mount(vNode.children, vNode.type, context, false);
        return vNode.dom = mountText(createVoidVNode(), parentDom);
      } // Development validation, in production we don't need to throw because it crashes anyway


    {
      if (_typeof(vNode) === 'object') {
        throwError("mount() received an object that's not a valid VNode, you should stringify it first, fix createVNode flags or call normalizeChildren. Object: \"" + JSON.stringify(vNode) + "\".");
      } else {
        throwError("mount() expects a valid VNode, instead it received an object with the type \"" + _typeof(vNode) + "\".");
      }
    }
  }

  function mountText(vNode, parentDom) {
    var dom = vNode.dom = document.createTextNode(vNode.children);

    if (!isNull(parentDom)) {
      appendChild(parentDom, dom);
    }

    return dom;
  }

  function mountElement(vNode, parentDom, context, isSVG) {
    var flags = vNode.flags;
    var children = vNode.children;
    var props = vNode.props;
    var className = vNode.className;
    var ref = vNode.ref;
    var childFlags = vNode.childFlags;
    isSVG = isSVG || (flags & 32
    /* SvgElement */
    ) > 0;
    var dom = documentCreateElement(vNode.type, isSVG);
    vNode.dom = dom;

    if (!isNullOrUndef(className) && className !== '') {
      if (isSVG) {
        dom.setAttribute('class', className);
      } else {
        dom.className = className;
      }
    }

    {
      validateKeys(vNode);
    }

    if (!isNull(parentDom)) {
      appendChild(parentDom, dom);
    }

    if ((childFlags & 1
    /* HasInvalidChildren */
    ) === 0) {
      var childrenIsSVG = isSVG === true && vNode.type !== 'foreignObject';

      if (childFlags === 2
      /* HasVNodeChildren */
      ) {
          mount(children, dom, context, childrenIsSVG);
        } else if (childFlags & 12
      /* MultipleChildren */
      ) {
          mountArrayChildren(children, dom, context, childrenIsSVG);
        }
    }

    if (!isNull(props)) {
      mountProps(vNode, flags, props, dom, isSVG);
    }

    {
      if (isString$1(ref)) {
        throwError('string "refs" are not supported in Inferno 1.0. Use callback "refs" instead.');
      }
    }

    if (isFunction$1(ref)) {
      mountRef(dom, ref);
    }

    return dom;
  }

  function mountArrayChildren(children, dom, context, isSVG) {
    for (var i = 0, len = children.length; i < len; i++) {
      var child = children[i];

      if (!isNull(child.dom)) {
        children[i] = child = directClone(child);
      }

      mount(child, dom, context, isSVG);
    }
  }

  function mountComponent(vNode, parentDom, context, isSVG, isClass) {
    var dom;
    var type = vNode.type;
    var props = vNode.props || EMPTY_OBJ;
    var ref = vNode.ref;

    if (isClass) {
      var instance = createClassComponentInstance(vNode, type, props, context);
      vNode.dom = dom = mount(instance.$LI, null, instance.$CX, isSVG);
      mountClassComponentCallbacks(vNode, ref, instance);
      instance.$UPD = false;
    } else {
      var input = handleComponentInput(type(props, context), vNode);
      vNode.children = input;
      vNode.dom = dom = mount(input, null, context, isSVG);
      mountFunctionalComponentCallbacks(props, ref, dom);
    }

    if (!isNull(parentDom)) {
      appendChild(parentDom, dom);
    }

    return dom;
  }

  function createClassMountCallback(instance) {
    return function () {
      instance.$UPD = true;
      instance.componentDidMount();
      instance.$UPD = false;
    };
  }

  function mountClassComponentCallbacks(vNode, ref, instance) {
    if (isFunction$1(ref)) {
      ref(instance);
    } else {
      {
        if (isStringOrNumber(ref)) {
          throwError('string "refs" are not supported in Inferno 1.0. Use callback "refs" instead.');
        } else if (!isNullOrUndef(ref) && isObject$1(ref) && vNode.flags & 4
        /* ComponentClass */
        ) {
            throwError('functional component lifecycle events are not supported on ES2015 class components.');
          }
      }
    }

    if (isFunction$1(instance.componentDidMount)) {
      LIFECYCLE.push(createClassMountCallback(instance));
    }
  }

  function createOnMountCallback(ref, dom, props) {
    return function () {
      return ref.onComponentDidMount(dom, props);
    };
  }

  function mountFunctionalComponentCallbacks(props, ref, dom) {
    if (!isNullOrUndef(ref)) {
      if (isFunction$1(ref.onComponentWillMount)) {
        ref.onComponentWillMount(props);
      }

      if (isFunction$1(ref.onComponentDidMount)) {
        LIFECYCLE.push(createOnMountCallback(ref, dom, props));
      }
    }
  }

  function mountRef(dom, value) {
    LIFECYCLE.push(function () {
      return value(dom);
    });
  }

  function hydrateComponent(vNode, dom, context, isSVG, isClass) {
    var type = vNode.type;
    var ref = vNode.ref;
    var props = vNode.props || EMPTY_OBJ;

    if (isClass) {
      var instance = createClassComponentInstance(vNode, type, props, context);
      var input = instance.$LI;
      hydrateVNode(input, dom, instance.$CX, isSVG);
      vNode.dom = input.dom;
      mountClassComponentCallbacks(vNode, ref, instance);
      instance.$UPD = false; // Mount finished allow going sync
    } else {
      var input$1 = handleComponentInput(type(props, context), vNode);
      hydrateVNode(input$1, dom, context, isSVG);
      vNode.children = input$1;
      vNode.dom = input$1.dom;
      mountFunctionalComponentCallbacks(props, ref, dom);
    }
  }

  function hydrateElement(vNode, dom, context, isSVG) {
    var children = vNode.children;
    var props = vNode.props;
    var className = vNode.className;
    var flags = vNode.flags;
    var ref = vNode.ref;
    isSVG = isSVG || (flags & 32
    /* SvgElement */
    ) > 0;

    if (dom.nodeType !== 1 || dom.tagName.toLowerCase() !== vNode.type) {
      {
        warning("Inferno hydration: Server-side markup doesn't match client-side markup or Initial render target is not empty");
      }
      var newDom = mountElement(vNode, null, context, isSVG);
      vNode.dom = newDom;
      replaceChild(dom.parentNode, newDom, dom);
    } else {
      vNode.dom = dom;
      var childNode = dom.firstChild;
      var childFlags = vNode.childFlags;

      if ((childFlags & 1
      /* HasInvalidChildren */
      ) === 0) {
        var nextSibling = null;

        while (childNode) {
          nextSibling = childNode.nextSibling;

          if (childNode.nodeType === 8) {
            if (childNode.data === '!') {
              dom.replaceChild(document.createTextNode(''), childNode);
            } else {
              dom.removeChild(childNode);
            }
          }

          childNode = nextSibling;
        }

        childNode = dom.firstChild;

        if (childFlags === 2
        /* HasVNodeChildren */
        ) {
            if (isNull(childNode)) {
              mount(children, dom, context, isSVG);
            } else {
              nextSibling = childNode.nextSibling;
              hydrateVNode(children, childNode, context, isSVG);
              childNode = nextSibling;
            }
          } else if (childFlags & 12
        /* MultipleChildren */
        ) {
            for (var i = 0, len = children.length; i < len; i++) {
              var child = children[i];

              if (isNull(childNode)) {
                mount(child, dom, context, isSVG);
              } else {
                nextSibling = childNode.nextSibling;
                hydrateVNode(child, childNode, context, isSVG);
                childNode = nextSibling;
              }
            }
          } // clear any other DOM nodes, there should be only a single entry for the root


        while (childNode) {
          nextSibling = childNode.nextSibling;
          dom.removeChild(childNode);
          childNode = nextSibling;
        }
      } else if (!isNull(dom.firstChild) && !isSamePropsInnerHTML(dom, props)) {
        dom.textContent = ''; // dom has content, but VNode has no children remove everything from DOM

        if (flags & 448
        /* FormElement */
        ) {
            // If element is form element, we need to clear defaultValue also
            dom.defaultValue = '';
          }
      }

      if (!isNull(props)) {
        mountProps(vNode, flags, props, dom, isSVG);
      }

      if (isNullOrUndef(className)) {
        if (dom.className !== '') {
          dom.removeAttribute('class');
        }
      } else if (isSVG) {
        dom.setAttribute('class', className);
      } else {
        dom.className = className;
      }

      if (isFunction$1(ref)) {
        mountRef(dom, ref);
      } else {
        {
          if (isString$1(ref)) {
            throwError('string "refs" are not supported in Inferno 1.0. Use callback "refs" instead.');
          }
        }
      }
    }
  }

  function hydrateText(vNode, dom) {
    if (dom.nodeType !== 3) {
      var newDom = mountText(vNode, null);
      vNode.dom = newDom;
      replaceChild(dom.parentNode, newDom, dom);
    } else {
      var text = vNode.children;

      if (dom.nodeValue !== text) {
        dom.nodeValue = text;
      }

      vNode.dom = dom;
    }
  }

  function hydrateVNode(vNode, dom, context, isSVG) {
    var flags = vNode.flags;

    if (flags & 14
    /* Component */
    ) {
        hydrateComponent(vNode, dom, context, isSVG, (flags & 4
        /* ComponentClass */
        ) > 0);
      } else if (flags & 481
    /* Element */
    ) {
        hydrateElement(vNode, dom, context, isSVG);
      } else if (flags & 16
    /* Text */
    ) {
        hydrateText(vNode, dom);
      } else if (flags & 512
    /* Void */
    ) {
        vNode.dom = dom;
      } else {
      {
        throwError("hydrate() expects a valid VNode, instead it received an object with the type \"" + _typeof(vNode) + "\".");
      }
      throwError();
    }
  }

  function hydrate(input, parentDom, callback) {
    var dom = parentDom.firstChild;

    if (!isNull(dom)) {
      if (!isInvalid(input)) {
        hydrateVNode(input, dom, EMPTY_OBJ, false);
      }

      dom = parentDom.firstChild; // clear any other DOM nodes, there should be only a single entry for the root

      while (dom = dom.nextSibling) {
        parentDom.removeChild(dom);
      }
    }

    if (LIFECYCLE.length > 0) {
      callAll(LIFECYCLE);
    }

    parentDom.$V = input;

    if (isFunction$1(callback)) {
      callback();
    }
  }

  function replaceWithNewNode(lastNode, nextNode, parentDom, context, isSVG) {
    unmount(lastNode);
    replaceChild(parentDom, mount(nextNode, null, context, isSVG), lastNode.dom);
  }

  function patch(lastVNode, nextVNode, parentDom, context, isSVG) {
    var nextFlags = nextVNode.flags | 0;

    if (lastVNode.flags !== nextFlags || nextFlags & 2048
    /* ReCreate */
    ) {
        replaceWithNewNode(lastVNode, nextVNode, parentDom, context, isSVG);
      } else if (nextFlags & 481
    /* Element */
    ) {
        patchElement(lastVNode, nextVNode, parentDom, context, isSVG, nextFlags);
      } else if (nextFlags & 14
    /* Component */
    ) {
        patchComponent(lastVNode, nextVNode, parentDom, context, isSVG, (nextFlags & 4
        /* ComponentClass */
        ) > 0);
      } else if (nextFlags & 16
    /* Text */
    ) {
        patchText(lastVNode, nextVNode);
      } else if (nextFlags & 512
    /* Void */
    ) {
        nextVNode.dom = lastVNode.dom;
      } else {
      patchPortal(lastVNode, nextVNode, context);
    }
  }

  function patchContentEditableChildren(dom, nextVNode) {
    if (dom.textContent !== nextVNode.children) {
      dom.textContent = nextVNode.children;
    }
  }

  function patchPortal(lastVNode, nextVNode, context) {
    var lastContainer = lastVNode.type;
    var nextContainer = nextVNode.type;
    var nextChildren = nextVNode.children;
    patchChildren(lastVNode.childFlags, nextVNode.childFlags, lastVNode.children, nextChildren, lastContainer, context, false);
    nextVNode.dom = lastVNode.dom;

    if (lastContainer !== nextContainer && !isInvalid(nextChildren)) {
      var node = nextChildren.dom;
      lastContainer.removeChild(node);
      nextContainer.appendChild(node);
    }
  }

  function patchElement(lastVNode, nextVNode, parentDom, context, isSVG, nextFlags) {
    var nextTag = nextVNode.type;

    if (lastVNode.type !== nextTag) {
      replaceWithNewNode(lastVNode, nextVNode, parentDom, context, isSVG);
    } else {
      var dom = lastVNode.dom;
      var lastProps = lastVNode.props;
      var nextProps = nextVNode.props;
      var isFormElement = false;
      var hasControlledValue = false;
      var nextPropsOrEmpty;
      nextVNode.dom = dom;
      isSVG = isSVG || (nextFlags & 32
      /* SvgElement */
      ) > 0; // inlined patchProps  -- starts --

      if (lastProps !== nextProps) {
        var lastPropsOrEmpty = lastProps || EMPTY_OBJ;
        nextPropsOrEmpty = nextProps || EMPTY_OBJ;

        if (nextPropsOrEmpty !== EMPTY_OBJ) {
          isFormElement = (nextFlags & 448
          /* FormElement */
          ) > 0;

          if (isFormElement) {
            hasControlledValue = isControlledFormElement(nextPropsOrEmpty);
          }

          for (var prop in nextPropsOrEmpty) {
            var lastValue = lastPropsOrEmpty[prop];
            var nextValue = nextPropsOrEmpty[prop];

            if (lastValue !== nextValue) {
              patchProp(prop, lastValue, nextValue, dom, isSVG, hasControlledValue, lastVNode);
            }
          }
        }

        if (lastPropsOrEmpty !== EMPTY_OBJ) {
          for (var prop$1 in lastPropsOrEmpty) {
            if (!nextPropsOrEmpty.hasOwnProperty(prop$1) && !isNullOrUndef(lastPropsOrEmpty[prop$1])) {
              patchProp(prop$1, lastPropsOrEmpty[prop$1], null, dom, isSVG, hasControlledValue, lastVNode);
            }
          }
        }
      }

      var lastChildren = lastVNode.children;
      var nextChildren = nextVNode.children;
      var nextRef = nextVNode.ref;
      var lastClassName = lastVNode.className;
      var nextClassName = nextVNode.className;
      {
        validateKeys(nextVNode);
      }

      if (nextFlags & 4096
      /* ContentEditable */
      ) {
          patchContentEditableChildren(dom, nextChildren);
        } else {
        patchChildren(lastVNode.childFlags, nextVNode.childFlags, lastChildren, nextChildren, dom, context, isSVG && nextTag !== 'foreignObject');
      }

      if (isFormElement) {
        processElement(nextFlags, nextVNode, dom, nextPropsOrEmpty, false, hasControlledValue);
      } // inlined patchProps  -- ends --


      if (lastClassName !== nextClassName) {
        if (isNullOrUndef(nextClassName)) {
          dom.removeAttribute('class');
        } else if (isSVG) {
          dom.setAttribute('class', nextClassName);
        } else {
          dom.className = nextClassName;
        }
      }

      if (isFunction$1(nextRef) && lastVNode.ref !== nextRef) {
        mountRef(dom, nextRef);
      } else {
        {
          if (isString$1(nextRef)) {
            throwError('string "refs" are not supported in Inferno 1.0. Use callback "refs" instead.');
          }
        }
      }
    }
  }

  function patchChildren(lastChildFlags, nextChildFlags, lastChildren, nextChildren, parentDOM, context, isSVG) {
    switch (lastChildFlags) {
      case 2
      /* HasVNodeChildren */
      :
        switch (nextChildFlags) {
          case 2
          /* HasVNodeChildren */
          :
            patch(lastChildren, nextChildren, parentDOM, context, isSVG);
            break;

          case 1
          /* HasInvalidChildren */
          :
            remove$3(lastChildren, parentDOM);
            break;

          default:
            remove$3(lastChildren, parentDOM);
            mountArrayChildren(nextChildren, parentDOM, context, isSVG);
            break;
        }

        break;

      case 1
      /* HasInvalidChildren */
      :
        switch (nextChildFlags) {
          case 2
          /* HasVNodeChildren */
          :
            mount(nextChildren, parentDOM, context, isSVG);
            break;

          case 1
          /* HasInvalidChildren */
          :
            break;

          default:
            mountArrayChildren(nextChildren, parentDOM, context, isSVG);
            break;
        }

        break;

      default:
        if (nextChildFlags & 12
        /* MultipleChildren */
        ) {
            var lastLength = lastChildren.length;
            var nextLength = nextChildren.length; // Fast path's for both algorithms

            if (lastLength === 0) {
              if (nextLength > 0) {
                mountArrayChildren(nextChildren, parentDOM, context, isSVG);
              }
            } else if (nextLength === 0) {
              removeAllChildren(parentDOM, lastChildren);
            } else if (nextChildFlags === 8
            /* HasKeyedChildren */
            && lastChildFlags === 8
            /* HasKeyedChildren */
            ) {
                patchKeyedChildren(lastChildren, nextChildren, parentDOM, context, isSVG, lastLength, nextLength);
              } else {
              patchNonKeyedChildren(lastChildren, nextChildren, parentDOM, context, isSVG, lastLength, nextLength);
            }
          } else if (nextChildFlags === 1
        /* HasInvalidChildren */
        ) {
            removeAllChildren(parentDOM, lastChildren);
          } else if (nextChildFlags === 2
        /* HasVNodeChildren */
        ) {
            removeAllChildren(parentDOM, lastChildren);
            mount(nextChildren, parentDOM, context, isSVG);
          }

        break;
    }
  }

  function updateClassComponent(instance, nextState, nextVNode, nextProps, parentDom, context, isSVG, force, fromSetState) {
    var lastState = instance.state;
    var lastProps = instance.props;
    nextVNode.children = instance;
    var renderOutput;

    if (instance.$UN) {
      {
        warning('Inferno Error: Can only update a mounted or mounting component. This usually means you called setState() or forceUpdate() on an unmounted component. This is a no-op.');
      }
      return;
    }

    if (lastProps !== nextProps || nextProps === EMPTY_OBJ) {
      if (!fromSetState && isFunction$1(instance.componentWillReceiveProps)) {
        instance.$BR = true;
        instance.componentWillReceiveProps(nextProps, context); // If instance component was removed during its own update do nothing.

        if (instance.$UN) {
          return;
        }

        instance.$BR = false;
      }

      if (instance.$PSS) {
        nextState = combineFrom(nextState, instance.$PS);
        instance.$PSS = false;
        instance.$PS = null;
      }
    }
    /* Update if scu is not defined, or it returns truthy value or force */


    var hasSCU = Boolean(instance.shouldComponentUpdate);

    if (force || !hasSCU || hasSCU && instance.shouldComponentUpdate(nextProps, nextState, context)) {
      if (isFunction$1(instance.componentWillUpdate)) {
        instance.$BS = true;
        instance.componentWillUpdate(nextProps, nextState, context);
        instance.$BS = false;
      }

      instance.props = nextProps;
      instance.state = nextState;
      instance.context = context;

      if (isFunction$1(options.beforeRender)) {
        options.beforeRender(instance);
      }

      renderOutput = instance.render(nextProps, nextState, context);

      if (isFunction$1(options.afterRender)) {
        options.afterRender(instance);
      }

      var didUpdate = renderOutput !== NO_OP;
      var childContext;

      if (isFunction$1(instance.getChildContext)) {
        childContext = instance.getChildContext();
      }

      if (isNullOrUndef(childContext)) {
        childContext = context;
      } else {
        childContext = combineFrom(context, childContext);
      }

      instance.$CX = childContext;

      if (didUpdate) {
        var lastInput = instance.$LI;
        var nextInput = handleComponentInput(renderOutput, nextVNode);
        patch(lastInput, nextInput, parentDom, childContext, isSVG);
        instance.$LI = nextInput;

        if (isFunction$1(instance.componentDidUpdate)) {
          instance.componentDidUpdate(lastProps, lastState);
        }
      }
    } else {
      instance.props = nextProps;
      instance.state = nextState;
      instance.context = context;
    }

    nextVNode.dom = instance.$LI.dom;
  }

  function patchComponent(lastVNode, nextVNode, parentDom, context, isSVG, isClass) {
    var nextType = nextVNode.type;
    var lastKey = lastVNode.key;
    var nextKey = nextVNode.key;

    if (lastVNode.type !== nextType || lastKey !== nextKey) {
      replaceWithNewNode(lastVNode, nextVNode, parentDom, context, isSVG);
    } else {
      var nextProps = nextVNode.props || EMPTY_OBJ;

      if (isClass) {
        var instance = lastVNode.children;
        instance.$UPD = true;
        instance.$V = nextVNode;
        updateClassComponent(instance, instance.state, nextVNode, nextProps, parentDom, context, isSVG, false, false);
        instance.$UPD = false;
      } else {
        var shouldUpdate = true;
        var lastProps = lastVNode.props;
        var nextHooks = nextVNode.ref;
        var nextHooksDefined = !isNullOrUndef(nextHooks);
        var lastInput = lastVNode.children;
        nextVNode.dom = lastVNode.dom;
        nextVNode.children = lastInput;

        if (nextHooksDefined && isFunction$1(nextHooks.onComponentShouldUpdate)) {
          shouldUpdate = nextHooks.onComponentShouldUpdate(lastProps, nextProps);
        }

        if (shouldUpdate !== false) {
          if (nextHooksDefined && isFunction$1(nextHooks.onComponentWillUpdate)) {
            nextHooks.onComponentWillUpdate(lastProps, nextProps);
          }

          var nextInput = nextType(nextProps, context);

          if (nextInput !== NO_OP) {
            nextInput = handleComponentInput(nextInput, nextVNode);
            patch(lastInput, nextInput, parentDom, context, isSVG);
            nextVNode.children = nextInput;
            nextVNode.dom = nextInput.dom;

            if (nextHooksDefined && isFunction$1(nextHooks.onComponentDidUpdate)) {
              nextHooks.onComponentDidUpdate(lastProps, nextProps);
            }
          }
        } else if (lastInput.flags & 14
        /* Component */
        ) {
            lastInput.parentVNode = nextVNode;
          }
      }
    }
  }

  function patchText(lastVNode, nextVNode) {
    var nextText = nextVNode.children;
    var dom = lastVNode.dom;

    if (nextText !== lastVNode.children) {
      dom.nodeValue = nextText;
    }

    nextVNode.dom = dom;
  }

  function patchNonKeyedChildren(lastChildren, nextChildren, dom, context, isSVG, lastChildrenLength, nextChildrenLength) {
    var commonLength = lastChildrenLength > nextChildrenLength ? nextChildrenLength : lastChildrenLength;
    var i = 0;
    var nextChild;
    var lastChild;

    for (; i < commonLength; i++) {
      nextChild = nextChildren[i];
      lastChild = lastChildren[i];

      if (nextChild.dom) {
        nextChild = nextChildren[i] = directClone(nextChild);
      }

      patch(lastChild, nextChild, dom, context, isSVG);
      lastChildren[i] = nextChild;
    }

    if (lastChildrenLength < nextChildrenLength) {
      for (i = commonLength; i < nextChildrenLength; i++) {
        nextChild = nextChildren[i];

        if (nextChild.dom) {
          nextChild = nextChildren[i] = directClone(nextChild);
        }

        mount(nextChild, dom, context, isSVG);
      }
    } else if (lastChildrenLength > nextChildrenLength) {
      for (i = commonLength; i < lastChildrenLength; i++) {
        remove$3(lastChildren[i], dom);
      }
    }
  }

  function patchKeyedChildren(a, b, dom, context, isSVG, aLength, bLength) {
    var aEnd = aLength - 1;
    var bEnd = bLength - 1;
    var i;
    var j = 0;
    var aNode = a[j];
    var bNode = b[j];
    var nextPos; // Step 1
    // tslint:disable-next-line

    outer: {
      // Sync nodes with the same key at the beginning.
      while (aNode.key === bNode.key) {
        if (bNode.dom) {
          b[j] = bNode = directClone(bNode);
        }

        patch(aNode, bNode, dom, context, isSVG);
        a[j] = bNode;
        j++;

        if (j > aEnd || j > bEnd) {
          break outer;
        }

        aNode = a[j];
        bNode = b[j];
      }

      aNode = a[aEnd];
      bNode = b[bEnd]; // Sync nodes with the same key at the end.

      while (aNode.key === bNode.key) {
        if (bNode.dom) {
          b[bEnd] = bNode = directClone(bNode);
        }

        patch(aNode, bNode, dom, context, isSVG);
        a[aEnd] = bNode;
        aEnd--;
        bEnd--;

        if (j > aEnd || j > bEnd) {
          break outer;
        }

        aNode = a[aEnd];
        bNode = b[bEnd];
      }
    }

    if (j > aEnd) {
      if (j <= bEnd) {
        nextPos = bEnd + 1;
        var nextNode = nextPos < bLength ? b[nextPos].dom : null;

        while (j <= bEnd) {
          bNode = b[j];

          if (bNode.dom) {
            b[j] = bNode = directClone(bNode);
          }

          j++;
          insertOrAppend(dom, mount(bNode, null, context, isSVG), nextNode);
        }
      }
    } else if (j > bEnd) {
      while (j <= aEnd) {
        remove$3(a[j++], dom);
      }
    } else {
      var aStart = j;
      var bStart = j;
      var aLeft = aEnd - j + 1;
      var bLeft = bEnd - j + 1;
      var sources = [];

      for (i = 0; i < bLeft; i++) {
        sources.push(0);
      } // Keep track if its possible to remove whole DOM using textContent = '';


      var canRemoveWholeContent = aLeft === aLength;
      var moved = false;
      var pos = 0;
      var patched = 0; // When sizes are small, just loop them through

      if (bLength < 4 || (aLeft | bLeft) < 32) {
        for (i = aStart; i <= aEnd; i++) {
          aNode = a[i];

          if (patched < bLeft) {
            for (j = bStart; j <= bEnd; j++) {
              bNode = b[j];

              if (aNode.key === bNode.key) {
                sources[j - bStart] = i + 1;

                if (canRemoveWholeContent) {
                  canRemoveWholeContent = false;

                  while (i > aStart) {
                    remove$3(a[aStart++], dom);
                  }
                }

                if (pos > j) {
                  moved = true;
                } else {
                  pos = j;
                }

                if (bNode.dom) {
                  b[j] = bNode = directClone(bNode);
                }

                patch(aNode, bNode, dom, context, isSVG);
                patched++;
                break;
              }
            }

            if (!canRemoveWholeContent && j > bEnd) {
              remove$3(aNode, dom);
            }
          } else if (!canRemoveWholeContent) {
            remove$3(aNode, dom);
          }
        }
      } else {
        var keyIndex = {}; // Map keys by their index

        for (i = bStart; i <= bEnd; i++) {
          keyIndex[b[i].key] = i;
        } // Try to patch same keys


        for (i = aStart; i <= aEnd; i++) {
          aNode = a[i];

          if (patched < bLeft) {
            j = keyIndex[aNode.key];

            if (j !== void 0) {
              if (canRemoveWholeContent) {
                canRemoveWholeContent = false;

                while (i > aStart) {
                  remove$3(a[aStart++], dom);
                }
              }

              bNode = b[j];
              sources[j - bStart] = i + 1;

              if (pos > j) {
                moved = true;
              } else {
                pos = j;
              }

              if (bNode.dom) {
                b[j] = bNode = directClone(bNode);
              }

              patch(aNode, bNode, dom, context, isSVG);
              patched++;
            } else if (!canRemoveWholeContent) {
              remove$3(aNode, dom);
            }
          } else if (!canRemoveWholeContent) {
            remove$3(aNode, dom);
          }
        }
      } // fast-path: if nothing patched remove all old and add all new


      if (canRemoveWholeContent) {
        removeAllChildren(dom, a);
        mountArrayChildren(b, dom, context, isSVG);
      } else {
        if (moved) {
          var seq = lis_algorithm(sources);
          j = seq.length - 1;

          for (i = bLeft - 1; i >= 0; i--) {
            if (sources[i] === 0) {
              pos = i + bStart;
              bNode = b[pos];

              if (bNode.dom) {
                b[pos] = bNode = directClone(bNode);
              }

              nextPos = pos + 1;
              insertOrAppend(dom, mount(bNode, null, context, isSVG), nextPos < bLength ? b[nextPos].dom : null);
            } else if (j < 0 || i !== seq[j]) {
              pos = i + bStart;
              bNode = b[pos];
              nextPos = pos + 1;
              insertOrAppend(dom, bNode.dom, nextPos < bLength ? b[nextPos].dom : null);
            } else {
              j--;
            }
          }
        } else if (patched !== bLeft) {
          // when patched count doesn't match b length we need to insert those new ones
          // loop backwards so we can use insertBefore
          for (i = bLeft - 1; i >= 0; i--) {
            if (sources[i] === 0) {
              pos = i + bStart;
              bNode = b[pos];

              if (bNode.dom) {
                b[pos] = bNode = directClone(bNode);
              }

              nextPos = pos + 1;
              insertOrAppend(dom, mount(bNode, null, context, isSVG), nextPos < bLength ? b[nextPos].dom : null);
            }
          }
        }
      }
    }
  } // https://en.wikipedia.org/wiki/Longest_increasing_subsequence


  function lis_algorithm(arr) {
    var p = arr.slice();
    var result = [0];
    var i;
    var j;
    var u;
    var v;
    var c;
    var len = arr.length;

    for (i = 0; i < len; i++) {
      var arrI = arr[i];

      if (arrI !== 0) {
        j = result[result.length - 1];

        if (arr[j] < arrI) {
          p[i] = j;
          result.push(i);
          continue;
        }

        u = 0;
        v = result.length - 1;

        while (u < v) {
          c = (u + v) / 2 | 0;

          if (arr[result[c]] < arrI) {
            u = c + 1;
          } else {
            v = c;
          }
        }

        if (arrI < arr[result[u]]) {
          if (u > 0) {
            p[i] = result[u - 1];
          }

          result[u] = i;
        }
      }
    }

    u = result.length;
    v = result[u - 1];

    while (u-- > 0) {
      result[u] = v;
      v = p[v];
    }

    return result;
  }

  {
    if (isBrowser && document.body === null) {
      warning('Inferno warning: you cannot initialize inferno without "document.body". Wait on "DOMContentLoaded" event, add script to bottom of body, or use async/defer attributes on script tag.');
    }
  }
  var documentBody = isBrowser ? document.body : null;

  function render(input, parentDom, callback) {
    // Development warning
    {
      if (documentBody === parentDom) {
        throwError('you cannot render() to the "document.body". Use an empty element as a container instead.');
      }
    }

    if (input === NO_OP) {
      return;
    }

    var rootInput = parentDom.$V;

    if (isNullOrUndef(rootInput)) {
      if (!isInvalid(input)) {
        if (input.dom) {
          input = directClone(input);
        }

        if (isNull(parentDom.firstChild)) {
          mount(input, parentDom, EMPTY_OBJ, false);
          parentDom.$V = input;
        } else {
          hydrate(input, parentDom);
        }

        rootInput = input;
      }
    } else {
      if (isNullOrUndef(input)) {
        remove$3(rootInput, parentDom);
        parentDom.$V = null;
      } else {
        if (input.dom) {
          input = directClone(input);
        }

        patch(rootInput, input, parentDom, EMPTY_OBJ, false);
        rootInput = parentDom.$V = input;
      }
    }

    if (LIFECYCLE.length > 0) {
      callAll(LIFECYCLE);
    }

    if (isFunction$1(callback)) {
      callback();
    }

    if (isFunction$1(options.renderComplete)) {
      options.renderComplete(rootInput);
    }

    if (rootInput && rootInput.flags & 14
    /* Component */
    ) {
        return rootInput.children;
      }
  }

  var resolvedPromise = typeof Promise === 'undefined' ? null : Promise.resolve(); // raf.bind(window) is needed to work around bug in IE10-IE11 strict mode (TypeError: Invalid calling object)

  var fallbackMethod = typeof requestAnimationFrame === 'undefined' ? setTimeout : requestAnimationFrame.bind(window);

  function nextTick(fn) {
    if (resolvedPromise) {
      return resolvedPromise.then(fn);
    }

    return fallbackMethod(fn);
  }

  function queueStateChanges(component, newState, callback, force) {
    if (isFunction$1(newState)) {
      newState = newState(component.state, component.props, component.context);
    }

    var pending = component.$PS;

    if (isNullOrUndef(pending)) {
      component.$PS = newState;
    } else {
      for (var stateKey in newState) {
        pending[stateKey] = newState[stateKey];
      }
    }

    if (!component.$PSS && !component.$BR) {
      if (!component.$UPD) {
        component.$PSS = true;
        component.$UPD = true;
        applyState(component, force, callback);
        component.$UPD = false;
      } else {
        // Async
        var queue = component.$QU;

        if (isNull(queue)) {
          queue = component.$QU = [];
          nextTick(promiseCallback(component, queue));
        }

        if (isFunction$1(callback)) {
          queue.push(callback);
        }
      }
    } else {
      component.$PSS = true;

      if (component.$BR && isFunction$1(callback)) {
        LIFECYCLE.push(callback.bind(component));
      }
    }
  }

  function promiseCallback(component, queue) {
    return function () {
      component.$QU = null;
      component.$UPD = true;
      applyState(component, false, function () {
        for (var i = 0, len = queue.length; i < len; i++) {
          queue[i].call(component);
        }
      });
      component.$UPD = false;
    };
  }

  function applyState(component, force, callback) {
    if (component.$UN) {
      return;
    }

    if (force || !component.$BR) {
      component.$PSS = false;
      var pendingState = component.$PS;
      var prevState = component.state;
      var nextState = combineFrom(prevState, pendingState);
      var props = component.props;
      var context = component.context;
      component.$PS = null;
      var vNode = component.$V;
      var lastInput = component.$LI;
      var parentDom = lastInput.dom && lastInput.dom.parentNode;
      updateClassComponent(component, nextState, vNode, props, parentDom, context, (vNode.flags & 32
      /* SvgElement */
      ) > 0, force, true);

      if (component.$UN) {
        return;
      }

      if ((component.$LI.flags & 1024
      /* Portal */
      ) === 0) {
        var dom = component.$LI.dom;

        while (!isNull(vNode = vNode.parentVNode)) {
          if ((vNode.flags & 14
          /* Component */
          ) > 0) {
            vNode.dom = dom;
          }
        }
      }

      if (LIFECYCLE.length > 0) {
        callAll(LIFECYCLE);
      }
    } else {
      component.state = component.$PS;
      component.$PS = null;
    }

    if (isFunction$1(callback)) {
      callback.call(component);
    }
  }

  var Component = function Component(props, context) {
    this.state = null; // Internal properties

    this.$BR = false; // BLOCK RENDER

    this.$BS = true; // BLOCK STATE

    this.$PSS = false; // PENDING SET STATE

    this.$PS = null; // PENDING STATE (PARTIAL or FULL)

    this.$LI = null; // LAST INPUT

    this.$V = null; // VNODE

    this.$UN = false; // UNMOUNTED

    this.$CX = null; // CHILDCONTEXT

    this.$UPD = true; // UPDATING

    this.$QU = null; // QUEUE

    /** @type {object} */

    this.props = props || EMPTY_OBJ;
    /** @type {object} */

    this.context = context || EMPTY_OBJ; // context should not be mutable
  };

  Component.prototype.forceUpdate = function forceUpdate(callback) {
    if (this.$UN) {
      return;
    } // Do not allow double render during force update


    queueStateChanges(this, {}, callback, true);
  };

  Component.prototype.setState = function setState(newState, callback) {
    if (this.$UN) {
      return;
    }

    if (!this.$BS) {
      queueStateChanges(this, newState, callback, false);
    } else {
      // Development warning
      {
        throwError('cannot update state via setState() in componentWillUpdate() or constructor.');
      }
      return;
    }
  }; // tslint:disable-next-line:no-empty


  Component.prototype.render = function render(_nextProps, _nextState, _nextContext) {};
  {
    /* tslint:disable-next-line:no-empty */
    var testFunc = function testFn() {};
    /* tslint:disable-next-line*/


    console.info('Inferno is in development mode.');

    if ((testFunc.name || testFunc.toString()).indexOf('testFn') === -1) {
      warning("It looks like you're using a minified copy of the development build " + 'of Inferno. When deploying Inferno apps to production, make sure to use ' + 'the production build which skips development warnings and is faster. ' + 'See http://infernojs.org for more details.');
    }
  }

  function _typeof$3(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$3 = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$3 = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$3(obj);
  }

  function _classCallCheck$8(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$7(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$7(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$7(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$7(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$2(self, call) {
    if (call && (_typeof$3(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$2(self);
  }

  function _getPrototypeOf$2(o) {
    _getPrototypeOf$2 = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$2(o);
  }

  function _assertThisInitialized$2(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$2(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$2(subClass, superClass);
  }

  function _setPrototypeOf$2(o, p) {
    _setPrototypeOf$2 = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$2(o, p);
  }

  var TableComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$2(TableComponent, _Component);

    function TableComponent(props) {
      var _this;

      _classCallCheck$8(this, TableComponent);

      _this = _possibleConstructorReturn$2(this, _getPrototypeOf$2(TableComponent).call(this, props));
      var injector = _this._injector = props.injector;
      _this._sheet = injector.get('sheet');
      _this._changeSupport = injector.get('changeSupport');
      _this._components = injector.get('components');
      _this.onElementsChanged = _this.onElementsChanged.bind(_assertThisInitialized$2(_this));
      return _this;
    }

    _createClass$7(TableComponent, [{
      key: "onElementsChanged",
      value: function onElementsChanged() {
        this.forceUpdate();
      }
    }, {
      key: "getChildContext",
      value: function getChildContext() {
        return {
          changeSupport: this._changeSupport,
          components: this._components,
          injector: this._injector
        };
      }
    }, {
      key: "componentWillMount",
      value: function componentWillMount() {
        var _this$_sheet$getRoot = this._sheet.getRoot(),
            id = _this$_sheet$getRoot.id;

        this._changeSupport.onElementsChanged(id, this.onElementsChanged);
      }
    }, {
      key: "componentWillUnmount",
      value: function componentWillUnmount() {
        var _this$_sheet$getRoot2 = this._sheet.getRoot(),
            id = _this$_sheet$getRoot2.id;

        this._changeSupport.offElementsChanged(id, this.onElementsChanged);
      }
    }, {
      key: "render",
      value: function render() {
        var _this$_sheet$getRoot3 = this._sheet.getRoot(),
            rows = _this$_sheet$getRoot3.rows,
            cols = _this$_sheet$getRoot3.cols;

        var beforeTableComponents = this._components.getComponents('table.before');

        var afterTableComponents = this._components.getComponents('table.after');

        var Head = this._components.getComponent('table.head');

        var Body = this._components.getComponent('table.body');

        var Foot = this._components.getComponent('table.foot');

        return createVNode(1, "div", "tjs-container", [beforeTableComponents && beforeTableComponents.map(function (Component, index) {
          return createComponentVNode(2, Component, null, index);
        }), createVNode(1, "div", "tjs-table-container", createVNode(1, "table", "tjs-table", [Head && createComponentVNode(2, Head, {
          "rows": rows,
          "cols": cols
        }), Body && createComponentVNode(2, Body, {
          "rows": rows,
          "cols": cols
        }), Foot && createComponentVNode(2, Foot, {
          "rows": rows,
          "cols": cols
        })], 0), 2), afterTableComponents && afterTableComponents.map(function (Component, index) {
          return createComponentVNode(2, Component, null, index);
        })], 0);
      }
    }]);

    return TableComponent;
  }(Component);

  function _classCallCheck$9(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$8(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$8(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$8(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$8(Constructor, staticProps);
    return Constructor;
  }

  var Renderer =
  /*#__PURE__*/
  function () {
    function Renderer(changeSupport, components, config, eventBus, injector) {
      _classCallCheck$9(this, Renderer);

      var container = config.container;
      this._container = container;
      eventBus.on('root.added', function () {
        render(createComponentVNode(2, TableComponent, {
          "injector": injector
        }), container);
      });
      eventBus.on('root.remove', function () {
        render(null, container);
      });
    }

    _createClass$8(Renderer, [{
      key: "getContainer",
      value: function getContainer() {
        return this._container;
      }
    }]);

    return Renderer;
  }();
  Renderer.$inject = ['changeSupport', 'components', 'config.renderer', 'eventBus', 'injector'];

  var renderModule = {
    __init__: ['changeSupport', 'components', 'renderer'],
    changeSupport: ['type', ChangeSupport],
    components: ['type', Components],
    renderer: ['type', Renderer]
  };

  function _classCallCheck$a(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$9(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$9(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$9(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$9(Constructor, staticProps);
    return Constructor;
  }

  var Sheet =
  /*#__PURE__*/
  function () {
    function Sheet(elementRegistry, eventBus) {
      var _this = this;

      _classCallCheck$a(this, Sheet);

      this._elementRegistry = elementRegistry;
      this._eventBus = eventBus;
      this._root = null;
      eventBus.on('table.clear', function () {
        _this.setRoot(null);
      });
    }

    _createClass$9(Sheet, [{
      key: "setRoot",
      value: function setRoot(root) {
        if (this._root) {
          var oldRoot = this._root;

          this._eventBus.fire('root.remove', {
            root: oldRoot
          });

          this._root = null;

          this._eventBus.fire('root.removed', {
            root: oldRoot
          });
        }

        if (root) {
          this._eventBus.fire('root.add', {
            root: root
          });
        }

        this._root = root;

        if (root) {
          this._eventBus.fire('root.added', {
            root: root
          });
        }
      }
    }, {
      key: "getRoot",
      value: function getRoot() {
        if (!this._root) {
          this.setRoot({
            id: '__implicitroot',
            rows: [],
            cols: []
          });
        }

        return this._root;
      }
      /**
       * Add row to sheet.
       *
       * @param {Object} row - Row.
       */

    }, {
      key: "addRow",
      value: function addRow(row, index) {
        var _this2 = this;

        var root = this.getRoot();

        if (root.cols.length != row.cells.length) {
          throw new Error('number of cells is not equal to number of cols');
        }

        if (typeof index === 'undefined') {
          index = root.rows.length;
        }

        addAtIndex(index, root.rows, row);
        row.root = root;

        this._elementRegistry.add(row);

        row.cells.forEach(function (cell, idx) {
          _this2._elementRegistry.add(cell);

          cell.row = row;
          cell.col = root.cols[idx];
          addAtIndex(index, root.cols[idx].cells, cell);
        });

        this._eventBus.fire('row.add', {
          row: row
        });

        return row;
      }
      /**
       * Remove row from sheet.
       *
       * @param {Object|string} row - Row or row ID.
       */

    }, {
      key: "removeRow",
      value: function removeRow(row) {
        var _this3 = this;

        var root = this.getRoot();

        if (typeof row === 'string') {
          row = this._elementRegistry.get(row);
        }

        var index = root.rows.indexOf(row);

        if (index === -1) {
          return;
        }

        removeAtIndex(index, root.rows);
        row.root = undefined;

        this._elementRegistry.remove(row);

        row.cells.forEach(function (cell, idx) {
          _this3._elementRegistry.remove(cell);

          cell.col = undefined;
          removeAtIndex(index, root.cols[idx].cells);
        });

        this._eventBus.fire('row.remove', {
          row: row
        });
      }
      /**
       * Add col to sheet.
       *
       * @param {Object} col
       * @param {Number} [index]
       */

    }, {
      key: "addCol",
      value: function addCol(col, index) {
        var _this4 = this;

        var root = this.getRoot();

        this._elementRegistry.add(col);

        if (root.rows.length != col.cells.length) {
          throw new Error('number of cells is not equal to number of rows');
        }

        if (typeof index === 'undefined') {
          index = root.cols.length;
        }

        addAtIndex(index, root.cols, col);
        col.root = root;
        col.cells.forEach(function (cell, idx) {
          _this4._elementRegistry.add(cell);

          cell.col = col;
          cell.row = root.rows[idx];
          addAtIndex(index, root.rows[idx].cells, cell);
        });

        this._eventBus.fire('col.add', {
          col: col
        });

        return col;
      }
      /**
       * Remove col from sheet.
       *
       * @param {Object|string} col - Col or col ID.
       */

    }, {
      key: "removeCol",
      value: function removeCol(col) {
        var _this5 = this;

        var root = this.getRoot();

        if (typeof col === 'string') {
          col = this._elementRegistry.get(col);
        }

        var index = root.cols.indexOf(col);

        if (index === -1) {
          return;
        }

        removeAtIndex(index, root.cols);
        col.root = undefined;

        this._elementRegistry.remove(col);

        col.cells.forEach(function (cell, idx) {
          _this5._elementRegistry.remove(cell);

          cell.row = undefined;
          removeAtIndex(index, root.rows[idx].cells);
        });

        this._eventBus.fire('col.remove', {
          col: col
        });
      }
    }, {
      key: "resized",
      value: function resized() {
        this._eventBus.fire('sheet.resized');
      }
    }]);

    return Sheet;
  }();
  Sheet.$inject = ['elementRegistry', 'eventBus']; // helpers /////////////

  /**
   * Insert value
   *
   * @param {number} index - Index to insert value at.
   * @param {Array} array - Array to insert value into.
   * @param {*} value - Value to insert.
   */

  function addAtIndex(index, array, value) {
    return array.splice(index, 0, value);
  }
  /**
   *
   * @param {number} index - Index to remove.
   * @param {Array} array - Array to remove from.
   */


  function removeAtIndex(index, array) {
    return array.splice(index, 1);
  }

  var core = {
    __depends__: [renderModule],
    __init__: ['elementFactory', 'sheet'],
    elementFactory: ['type', ElementFactory$1],
    elementRegistry: ['type', ElementRegistry$1],
    eventBus: ['type', EventBus],
    sheet: ['type', Sheet]
  };

  function _objectWithoutProperties$1(source, excluded) {
    if (source == null) return {};

    var target = _objectWithoutPropertiesLoose$1(source, excluded);

    var key, i;

    if (Object.getOwnPropertySymbols) {
      var sourceSymbolKeys = Object.getOwnPropertySymbols(source);

      for (i = 0; i < sourceSymbolKeys.length; i++) {
        key = sourceSymbolKeys[i];
        if (excluded.indexOf(key) >= 0) continue;
        if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue;
        target[key] = source[key];
      }
    }

    return target;
  }

  function _objectWithoutPropertiesLoose$1(source, excluded) {
    if (source == null) return {};
    var target = {};
    var sourceKeys = Object.keys(source);
    var key, i;

    for (i = 0; i < sourceKeys.length; i++) {
      key = sourceKeys[i];
      if (excluded.indexOf(key) >= 0) continue;
      target[key] = source[key];
    }

    return target;
  }

  function _classCallCheck$b(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$a(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$a(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$a(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$a(Constructor, staticProps);
    return Constructor;
  }

  var Table =
  /*#__PURE__*/
  function () {
    function Table() {
      var options = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

      _classCallCheck$b(this, Table);

      var injector = options.injector;

      if (!injector) {
        var _this$_init = this._init(options),
            modules = _this$_init.modules,
            config = _this$_init.config;

        injector = createInjector$1(config, modules);
      }

      this.get = injector.get;
      this.invoke = injector.invoke;
      this.get('eventBus').fire('table.init');
      this.get('eventBus').fire('diagram.init');
    }
    /**
     * Intialize table and return modules and config used for creation.
     *
     * @param  {Object} options
     *
     * @return {Object} { modules=[], config }
     */


    _createClass$a(Table, [{
      key: "_init",
      value: function _init(options) {
        var modules = options.modules,
            config = _objectWithoutProperties$1(options, ["modules"]);

        return {
          modules: modules,
          config: config
        };
      }
      /**
       * Destroys the table. This results in removing the attachment from the container.
       */

    }, {
      key: "destroy",
      value: function destroy() {
        var eventBus = this.get('eventBus');
        eventBus.fire('table.destroy');
        eventBus.fire('diagram.destroy');
      }
      /**
       * Clears the table. Should be used to reset the state of any stateful services.
       */

    }, {
      key: "clear",
      value: function clear() {
        var eventBus = this.get('eventBus');
        eventBus.fire('table.clear');
        eventBus.fire('diagram.clear');
      }
    }]);

    return Table;
  }(); // helpers /////////////

  function bootstrap$1(bootstrapModules) {
    var modules = [],
        components = [];

    function hasModule(m) {
      return modules.indexOf(m) >= 0;
    }

    function addModule(m) {
      modules.push(m);
    }

    function visit(m) {
      if (hasModule(m)) {
        return;
      }

      (m.__depends__ || []).forEach(visit);

      if (hasModule(m)) {
        return;
      }

      addModule(m);
      (m.__init__ || []).forEach(function (c) {
        components.push(c);
      });
    }

    bootstrapModules.forEach(visit);
    var injector = new Injector(modules);
    components.forEach(function (c) {
      try {
        // eagerly resolve component (fn or string)
        injector[typeof c === 'string' ? 'get' : 'invoke'](c);
      } catch (e) {
        console.error('Failed to instantiate component');
        console.error(e.stack);
        throw e;
      }
    });
    return injector;
  }

  function createInjector$1(config, modules) {
    var bootstrapModules = [{
      config: ['value', config]
    }, core].concat(modules || []);
    return bootstrap$1(bootstrapModules);
  }

  function elementToString(element) {
    if (!element) {
      return '<null>';
    }

    var id = element.id ? " id=\"".concat(element.id, "\"") : '';
    return "<".concat(element.$type).concat(id, " />");
  }

  function TableTreeWalker(handler, options) {
    function visit(element, ctx, definitions) {
      var gfx = element.gfx; // avoid multiple rendering of elements

      if (gfx) {
        throw new Error("already rendered ".concat(elementToString(element)));
      } // call handler


      return handler.element(element, ctx, definitions);
    }

    function visitTable(element) {
      return handler.table(element);
    } // Semantic handling //////////////////////


    function handleDecision(decision) {
      if (!decision.id) {
        decision.id = 'decision';
      }

      var table = decision.decisionLogic;

      if (table) {
        if (!table.output) {
          throw new Error("missing output for ".concat(elementToString(table)));
        }

        var ctx = visitTable(table);

        if (table.input) {
          handleClauses(table.input, ctx, table);
        }

        handleClauses(table.output, ctx, table); // if any input or output clauses (columns) were added
        // make sure that for each rule the according input/output entry is created

        handleRules(table.rule, ctx, table);
      } else {
        throw new Error("no table for ".concat(elementToString(decision)));
      }
    }

    function handleClauses(clauses, context, definitions) {
      forEach(clauses, function (e) {
        visit(e, context, definitions);
      });
    }

    function handleRules(rules, context, definitions) {
      forEach(rules, function (e) {
        visit(e, context, definitions);
        handleEntry(e.inputEntry, e);
        handleEntry(e.outputEntry, e);
      });
    }

    function handleEntry(entry, context, definitions) {
      forEach(entry, function (e) {
        visit(e, context, definitions);
      });
    } // API //////////////////////


    return {
      handleDecision: handleDecision
    };
  }

  /**
   * Import the decision table into a table.
   *
   * Errors and warnings are reported through the specified callback.
   *
   * @param  {decisionTable} decisionTable instance of DecisionTable
   * @param  {ModdleElement} decision moddle element
   * @param  {Function} done
   *         the callback, invoked with (err, [ warning ]) once the import is done
   */

  function importDecision(decisionTable, decision, done) {
    var importer = decisionTable.get('tableImporter'),
        eventBus = decisionTable.get('eventBus'),
        sheet = decisionTable.get('sheet');
    var hasModeling = decisionTable.get('modeling', false);
    var error,
        warnings = [];

    function render(decision) {
      var visitor = {
        create: function create(type, parent, clause, rule) {
          return importer.create(type, parent, clause, rule);
        },
        table: function table(element) {
          return importer.add(element);
        },
        element: function element(_element, parentShape, definitions) {
          return importer.add(_element, parentShape, definitions);
        },
        error: function error(message, context) {
          warnings.push({
            message: message,
            context: context
          });
        }
      };
      var walker = new TableTreeWalker(visitor, {
        canAddMissingEntries: hasModeling
      }); // import

      walker.handleDecision(decision);
    }

    eventBus.fire('import.render.start', {
      decision: decision
    });

    try {
      render(decision);
    } catch (e) {
      error = e;
    }

    eventBus.fire('import.render.complete', {
      error: error,
      warnings: warnings
    });
    eventBus.fire('elements.changed', {
      elements: [sheet.getRoot()]
    });
    done(error, warnings);
  }

  function _typeof$4(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$4 = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$4 = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$4(obj);
  }

  function ownKeys$2(object, enumerableOnly) {
    var keys = Object.keys(object);

    if (Object.getOwnPropertySymbols) {
      keys.push.apply(keys, Object.getOwnPropertySymbols(object));
    }

    if (enumerableOnly) keys = keys.filter(function (sym) {
      return Object.getOwnPropertyDescriptor(object, sym).enumerable;
    });
    return keys;
  }

  function _objectSpread$2(target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i] != null ? arguments[i] : {};

      if (i % 2) {
        ownKeys$2(source, true).forEach(function (key) {
          _defineProperty$2(target, key, source[key]);
        });
      } else if (Object.getOwnPropertyDescriptors) {
        Object.defineProperties(target, Object.getOwnPropertyDescriptors(source));
      } else {
        ownKeys$2(source).forEach(function (key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
    }

    return target;
  }

  function _defineProperty$2(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  function _toConsumableArray$2(arr) {
    return _arrayWithoutHoles$2(arr) || _iterableToArray$2(arr) || _nonIterableSpread$2();
  }

  function _nonIterableSpread$2() {
    throw new TypeError("Invalid attempt to spread non-iterable instance");
  }

  function _iterableToArray$2(iter) {
    if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter);
  }

  function _arrayWithoutHoles$2(arr) {
    if (Array.isArray(arr)) {
      for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) {
        arr2[i] = arr[i];
      }

      return arr2;
    }
  }

  function newSet() {
    return {
      elements: [],
      index: {}
    };
  }

  function add$1(set, element) {
    var elements = set.elements,
        index = set.index;

    if (index[element]) {
      return set;
    } else {
      return {
        elements: [].concat(_toConsumableArray$2(elements), [element]),
        index: _objectSpread$2({}, index, _defineProperty$2({}, element, true))
      };
    }
  }

  function join(set, separator) {
    return set.elements.join(separator);
  }

  function classNames() {
    var set = newSet();

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    args.forEach(function (item) {
      var type = _typeof$4(item);

      if (type === 'string' && item.length > 0) {
        set = add$1(set, item);
      } else if (type === 'object' && item !== null) {
        Object.keys(item).forEach(function (key) {
          var value = item[key];

          if (value) {
            set = add$1(set, key);
          }
        });
      }
    });
    return join(set, ' ');
  }

  function _toConsumableArray$3(arr) {
    return _arrayWithoutHoles$3(arr) || _iterableToArray$3(arr) || _nonIterableSpread$3();
  }

  function _nonIterableSpread$3() {
    throw new TypeError("Invalid attempt to spread non-iterable instance");
  }

  function _iterableToArray$3(iter) {
    if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter);
  }

  function _arrayWithoutHoles$3(arr) {
    if (Array.isArray(arr)) {
      for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) {
        arr2[i] = arr[i];
      }

      return arr2;
    }
  }

  function inject(component) {
    var Type = component.constructor;
    return injectType(Type, component);
  }
  function injectType(Type, component) {
    var annotation = Type.$inject;

    if (!annotation) {
      return;
    }

    var injector = component.context.injector;
    var setupFn = [].concat(_toConsumableArray$3(annotation), [function () {
      for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
        args[_key] = arguments[_key];
      }

      for (var idx in args) {
        var name = annotation[idx];
        var value = args[idx];
        component[name] = value;
      }
    }]);
    injector.invoke(setupFn);
  }

  /**
   * Composes a number of functions.
   *
   * All receive the the same arguments; the chain is interruped as soon
   * as one function returns a value.
   *
   * @param  {Object}    self
   * @param  {...Function} fns
   *
   * @return {Object}
   */
  function compose(self) {
    for (var _len = arguments.length, fns = new Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
      fns[_key - 1] = arguments[_key];
    }

    return function () {
      for (var _len2 = arguments.length, args = new Array(_len2), _key2 = 0; _key2 < _len2; _key2++) {
        args[_key2] = arguments[_key2];
      }

      var result;
      fns.forEach(function (fn) {
        result = fn.call.apply(fn, [self].concat(args));

        if (typeof result !== 'undefined') {
          return false;
        }
      });
      return result;
    }.bind(self);
  }

  /**
   * A Component and injection aware mixin mechanism.
   *
   * @param {Component} component
   * @param {Object|Function} mixinDef
   */

  function mixin(component, mixinDef) {
    Object.keys(mixinDef).forEach(function (key) {
      if (key === '$inject' || key === '__init') {
        return;
      }

      var mixinFn = mixinDef[key];

      if (key === 'constructor') {
        mixinFn.call(component, component.props, component.context);
      }

      var componentFn = component[key];

      if (typeof componentFn !== 'undefined') {
        if (typeof componentFn !== 'function') {
          throw new Error("failed to mixin <".concat(key, ">: cannot combine with non-fn component value"));
        }

        component[key] = compose(component, componentFn, mixinFn);
      } else {
        component[key] = mixinFn.bind(component);
      }
    });

    if ('$inject' in mixinDef) {
      injectType(mixinDef, component);
    } // call initializer


    if ('__init' in mixinDef) {
      mixinDef.__init.call(component, component.props, component.context);
    }
  }

  /**
   * A mixin to make an element _selection aware_.
   */

  var SelectionAware = {
    getSelectionClasses: function getSelectionClasses() {
      var _this$state = this.state,
          selected = _this$state.selected,
          selectedSecondary = _this$state.selectedSecondary,
          focussed = _this$state.focussed;
      return classNames({
        'selected': selected,
        'selected-secondary': selectedSecondary,
        'focussed': focussed
      });
    },
    selectionChanged: function selectionChanged(newSelection) {
      // newSelection = { selected, selectedSecondary, focussed }
      this.setState(newSelection);
    },
    componentWillUpdate: function componentWillUpdate(newProps) {
      if (newProps.elementId !== this.props.elementId) {
        this.updateSelectionSubscription(false);
      }
    },
    componentDidUpdate: function componentDidUpdate(oldProps) {
      if (oldProps.elementId !== this.props.elementId) {
        this.updateSelectionSubscription(true);
      }
    },
    componentDidMount: function componentDidMount() {
      this.updateSelectionSubscription(true);
    },
    componentWillUnmount: function componentWillUnmount() {
      this.updateSelectionSubscription(false);
    },
    updateSelectionSubscription: function updateSelectionSubscription(enable) {
      var elementId = this.props.elementId;

      if (!elementId) {
        return;
      }

      if (elementId) {
        this.eventBus[enable ? 'on' : 'off']("selection.".concat(elementId, ".changed"), this.selectionChanged);
      }
    }
  };
  SelectionAware.$inject = ['eventBus'];

  function _typeof$5(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$5 = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$5 = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$5(obj);
  }

  function ownKeys$3(object, enumerableOnly) {
    var keys = Object.keys(object);

    if (Object.getOwnPropertySymbols) {
      keys.push.apply(keys, Object.getOwnPropertySymbols(object));
    }

    if (enumerableOnly) keys = keys.filter(function (sym) {
      return Object.getOwnPropertyDescriptor(object, sym).enumerable;
    });
    return keys;
  }

  function _objectSpread$3(target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i] != null ? arguments[i] : {};

      if (i % 2) {
        ownKeys$3(source, true).forEach(function (key) {
          _defineProperty$3(target, key, source[key]);
        });
      } else if (Object.getOwnPropertyDescriptors) {
        Object.defineProperties(target, Object.getOwnPropertyDescriptors(source));
      } else {
        ownKeys$3(source).forEach(function (key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
    }

    return target;
  }

  function _defineProperty$3(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  function _objectWithoutProperties$2(source, excluded) {
    if (source == null) return {};

    var target = _objectWithoutPropertiesLoose$2(source, excluded);

    var key, i;

    if (Object.getOwnPropertySymbols) {
      var sourceSymbolKeys = Object.getOwnPropertySymbols(source);

      for (i = 0; i < sourceSymbolKeys.length; i++) {
        key = sourceSymbolKeys[i];
        if (excluded.indexOf(key) >= 0) continue;
        if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue;
        target[key] = source[key];
      }
    }

    return target;
  }

  function _objectWithoutPropertiesLoose$2(source, excluded) {
    if (source == null) return {};
    var target = {};
    var sourceKeys = Object.keys(source);
    var key, i;

    for (i = 0; i < sourceKeys.length; i++) {
      key = sourceKeys[i];
      if (excluded.indexOf(key) >= 0) continue;
      target[key] = source[key];
    }

    return target;
  }

  function _classCallCheck$c(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$b(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$b(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$b(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$b(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$3(self, call) {
    if (call && (_typeof$5(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$3(self);
  }

  function _getPrototypeOf$3(o) {
    _getPrototypeOf$3 = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$3(o);
  }

  function _assertThisInitialized$3(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$3(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$3(subClass, superClass);
  }

  function _setPrototypeOf$3(o, p) {
    _setPrototypeOf$3 = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$3(o, p);
  }

  var BaseCell =
  /*#__PURE__*/
  function (_Component) {
    _inherits$3(BaseCell, _Component);

    function BaseCell(props, context) {
      var _this;

      _classCallCheck$c(this, BaseCell);

      _this = _possibleConstructorReturn$3(this, _getPrototypeOf$3(BaseCell).call(this, props, context));
      mixin(_assertThisInitialized$3(_this), SelectionAware);
      inject(_assertThisInitialized$3(_this));
      return _this;
    }

    _createClass$b(BaseCell, [{
      key: "getRenderProps",
      value: function getRenderProps() {
        var _this$props = this.props,
            className = _this$props.className,
            elementId = _this$props.elementId,
            coords = _this$props.coords,
            props = _objectWithoutProperties$2(_this$props, ["className", "elementId", "coords"]);

        for (var _len = arguments.length, cls = new Array(_len), _key = 0; _key < _len; _key++) {
          cls[_key] = arguments[_key];
        }

        var baseProps = {
          className: classNames.apply(void 0, cls.concat([this.getSelectionClasses(), className]))
        };

        if (elementId) {
          baseProps['data-element-id'] = elementId;
        }

        if (coords) {
          baseProps['data-coords'] = coords;
        }

        return _objectSpread$3({}, baseProps, {}, props);
      }
    }]);

    return BaseCell;
  }(Component);

  function _typeof$6(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$6 = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$6 = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$6(obj);
  }

  function ownKeys$4(object, enumerableOnly) {
    var keys = Object.keys(object);

    if (Object.getOwnPropertySymbols) {
      keys.push.apply(keys, Object.getOwnPropertySymbols(object));
    }

    if (enumerableOnly) keys = keys.filter(function (sym) {
      return Object.getOwnPropertyDescriptor(object, sym).enumerable;
    });
    return keys;
  }

  function _objectSpread$4(target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i] != null ? arguments[i] : {};

      if (i % 2) {
        ownKeys$4(source, true).forEach(function (key) {
          _defineProperty$4(target, key, source[key]);
        });
      } else if (Object.getOwnPropertyDescriptors) {
        Object.defineProperties(target, Object.getOwnPropertyDescriptors(source));
      } else {
        ownKeys$4(source).forEach(function (key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
    }

    return target;
  }

  function _defineProperty$4(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  function _classCallCheck$d(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$c(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$c(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$c(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$c(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$4(self, call) {
    if (call && (_typeof$6(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$4(self);
  }

  function _assertThisInitialized$4(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _getPrototypeOf$4(o) {
    _getPrototypeOf$4 = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$4(o);
  }

  function _inherits$4(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$4(subClass, superClass);
  }

  function _setPrototypeOf$4(o, p) {
    _setPrototypeOf$4 = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$4(o, p);
  }

  var HeaderCell =
  /*#__PURE__*/
  function (_BaseCell) {
    _inherits$4(HeaderCell, _BaseCell);

    function HeaderCell(props, context) {
      var _this;

      _classCallCheck$d(this, HeaderCell);

      _this = _possibleConstructorReturn$4(this, _getPrototypeOf$4(HeaderCell).call(this, props, context));
      _this.state = {};
      return _this;
    }

    _createClass$c(HeaderCell, [{
      key: "render",
      value: function render() {
        var children = this.props.children;
        var props = this.getRenderProps('cell');
        return normalizeProps(createVNode(1, "td", null, children, 0, _objectSpread$4({}, props)));
      }
    }]);

    return HeaderCell;
  }(BaseCell);

  function ownKeys$5(object, enumerableOnly) {
    var keys = Object.keys(object);

    if (Object.getOwnPropertySymbols) {
      var symbols = Object.getOwnPropertySymbols(object);
      if (enumerableOnly) symbols = symbols.filter(function (sym) {
        return Object.getOwnPropertyDescriptor(object, sym).enumerable;
      });
      keys.push.apply(keys, symbols);
    }

    return keys;
  }

  function _objectSpread$5(target) {
    for (var i = 1; i < arguments.length; i++) {
      var source = arguments[i] != null ? arguments[i] : {};

      if (i % 2) {
        ownKeys$5(source, true).forEach(function (key) {
          _defineProperty$5(target, key, source[key]);
        });
      } else if (Object.getOwnPropertyDescriptors) {
        Object.defineProperties(target, Object.getOwnPropertyDescriptors(source));
      } else {
        ownKeys$5(source).forEach(function (key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
    }

    return target;
  }

  function _defineProperty$5(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  function _objectWithoutProperties$3(source, excluded) {
    if (source == null) return {};

    var target = _objectWithoutPropertiesLoose$3(source, excluded);

    var key, i;

    if (Object.getOwnPropertySymbols) {
      var sourceSymbolKeys = Object.getOwnPropertySymbols(source);

      for (i = 0; i < sourceSymbolKeys.length; i++) {
        key = sourceSymbolKeys[i];
        if (excluded.indexOf(key) >= 0) continue;
        if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue;
        target[key] = source[key];
      }
    }

    return target;
  }

  function _objectWithoutPropertiesLoose$3(source, excluded) {
    if (source == null) return {};
    var target = {};
    var sourceKeys = Object.keys(source);
    var key, i;

    for (i = 0; i < sourceKeys.length; i++) {
      key = sourceKeys[i];
      if (excluded.indexOf(key) >= 0) continue;
      target[key] = source[key];
    }

    return target;
  }
  /**
   * A simple slot extension, built upon the components service.
   *
   * @type {Object}
   */


  var ComponentWithSlots = {
    slotFill: function slotFill(slotProps, DefaultFill) {
      var type = slotProps.type,
          context = slotProps.context,
          props = _objectWithoutProperties$3(slotProps, ["type", "context"]);

      var Fill = this.components.getComponent(type, context) || DefaultFill;

      if (Fill) {
        return normalizeProps(createComponentVNode(2, Fill, _objectSpread$5({}, context, {}, props)));
      }

      return null;
    },
    slotFills: function slotFills(slotProps) {
      var type = slotProps.type,
          context = slotProps.context,
          props = _objectWithoutProperties$3(slotProps, ["type", "context"]);

      var fills = this.components.getComponents(type, context);
      return fills.map(function (Fill) {
        return normalizeProps(createComponentVNode(2, Fill, _objectSpread$5({}, context, {}, props)));
      });
    }
  };
  ComponentWithSlots.$inject = ['components'];

  function _typeof$7(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$7 = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$7 = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$7(obj);
  }

  function _classCallCheck$e(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$d(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$d(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$d(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$d(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$5(self, call) {
    if (call && (_typeof$7(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$5(self);
  }

  function _getPrototypeOf$5(o) {
    _getPrototypeOf$5 = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$5(o);
  }

  function _assertThisInitialized$5(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$5(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$5(subClass, superClass);
  }

  function _setPrototypeOf$5(o, p) {
    _setPrototypeOf$5 = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$5(o, p);
  }

  function _defineProperty$6(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }
  var MIN_WIDTH = 400;

  var AnnotationHeader =
  /*#__PURE__*/
  function (_Component) {
    _inherits$5(AnnotationHeader, _Component);

    function AnnotationHeader(props, context) {
      var _this;

      _classCallCheck$e(this, AnnotationHeader);

      _this = _possibleConstructorReturn$5(this, _getPrototypeOf$5(AnnotationHeader).call(this, props, context));

      _defineProperty$6(_assertThisInitialized$5(_this), "onElementsChanged", function () {
        _this.forceUpdate();
      });

      mixin(_assertThisInitialized$5(_this), ComponentWithSlots);
      inject(_assertThisInitialized$5(_this));
      return _this;
    }

    _createClass$d(AnnotationHeader, [{
      key: "componentDidMount",
      value: function componentDidMount() {
        this.changeSupport.onElementsChanged(this.getRoot(), this.onElementsChanged);
      }
    }, {
      key: "componentWillUnmount",
      value: function componentWillUnmount() {
        this.changeSupport.offElementsChanged(this.getRoot(), this.onElementsChanged);
      }
    }, {
      key: "getRoot",
      value: function getRoot() {
        return this.sheet.getRoot();
      }
    }, {
      key: "render",
      value: function render() {
        var decisionTable = this.getRoot();
        var annotationsWidth = decisionTable.businessObject.get('annotationsWidth');
        var width = (annotationsWidth || MIN_WIDTH) + 'px';
        return createVNode(1, "th", "annotation header", [this.slotFills({
          type: 'cell-inner',
          context: {
            cellType: 'annotations',
            col: this.sheet.getRoot(),
            minWidth: MIN_WIDTH
          }
        }), this.translate('Annotations')], 0, {
          "style": {
            width: width
          }
        });
      }
    }]);

    return AnnotationHeader;
  }(Component);
  AnnotationHeader.$inject = ['changeSupport', 'sheet', 'translate'];

  function AnnotationCell(props) {
    var row = props.row;
    var _row$businessObject = row.businessObject,
        id = _row$businessObject.id,
        description = _row$businessObject.description;
    return createComponentVNode(2, HeaderCell, {
      "className": "annotation",
      "elementId": id,
      children: description || '-'
    });
  }

  function AnnotationsProvider(components) {
    components.onGetComponent('cell', function (_ref) {
      var cellType = _ref.cellType;

      if (cellType === 'after-label-cells') {
        return AnnotationHeader;
      } else if (cellType === 'after-rule-cells') {
        return AnnotationCell;
      }
    });
  }
  AnnotationsProvider.$inject = ['components'];

  var annotationsModule = {
    __init__: ['annotationsProvider'],
    annotationsProvider: ['type', AnnotationsProvider]
  };

  function _toConsumableArray$4(arr) {
    return _arrayWithoutHoles$4(arr) || _iterableToArray$4(arr) || _nonIterableSpread$4();
  }

  function _nonIterableSpread$4() {
    throw new TypeError("Invalid attempt to spread non-iterable instance");
  }

  function _iterableToArray$4(iter) {
    if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter);
  }

  function _arrayWithoutHoles$4(arr) {
    if (Array.isArray(arr)) {
      for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) {
        arr2[i] = arr[i];
      }

      return arr2;
    }
  }

  function _classCallCheck$f(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$e(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$e(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$e(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$e(Constructor, staticProps);
    return Constructor;
  }

  function elementData$1(semantic, attrs) {
    return assign({
      id: semantic.id,
      type: semantic.$type,
      businessObject: semantic
    }, attrs);
  }

  var TableImporter =
  /*#__PURE__*/
  function () {
    function TableImporter(elementFactory, eventBus, sheet) {
      _classCallCheck$f(this, TableImporter);

      this._elementFactory = elementFactory;
      this._eventBus = eventBus;
      this._sheet = sheet;
    }
    /**
     * Add DMN element.
     */


    _createClass$e(TableImporter, [{
      key: "add",
      value: function add(semantic) {
        var _this = this;

        var element; // decision table

        if (is(semantic, 'dmn:DecisionTable')) {
          element = this._elementFactory.createRoot(elementData$1(semantic));

          this._sheet.setRoot(element);
        } // input clause
        else if (is(semantic, 'dmn:InputClause')) {
            element = this._elementFactory.createCol(elementData$1(semantic));

            this._sheet.addCol(element);
          } // output clause
          else if (is(semantic, 'dmn:OutputClause')) {
              element = this._elementFactory.createCol(elementData$1(semantic));

              this._sheet.addCol(element);
            } // rule
            else if (is(semantic, 'dmn:DecisionRule')) {
                if (!semantic.inputEntry) {
                  semantic.inputEntry = [];
                }

                if (!semantic.outputEntry) {
                  semantic.outputEntry = [];
                }

                var cells = [].concat(_toConsumableArray$4(semantic.inputEntry), _toConsumableArray$4(semantic.outputEntry)).map(function (entry) {
                  return _this._elementFactory.createCell(elementData$1(entry));
                });
                element = this._elementFactory.createRow(assign(elementData$1(semantic), {
                  cells: cells
                }));

                this._sheet.addRow(element);
              }

        this._eventBus.fire('dmnElement.added', {
          element: element
        });

        return element;
      }
    }]);

    return TableImporter;
  }();
  TableImporter.$inject = ['elementFactory', 'eventBus', 'sheet'];

  var importModule = {
    __depends__: [TranslateModule],
    tableImporter: ['type', TableImporter]
  };

  var coreModule = {
    __depends__: [importModule, renderModule]
  };

  function _typeof$8(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$8 = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$8 = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$8(obj);
  }

  function _classCallCheck$g(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$f(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$f(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$f(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$f(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$6(self, call) {
    if (call && (_typeof$8(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$6(self);
  }

  function _getPrototypeOf$6(o) {
    _getPrototypeOf$6 = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$6(o);
  }

  function _assertThisInitialized$6(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$6(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$6(subClass, superClass);
  }

  function _setPrototypeOf$6(o, p) {
    _setPrototypeOf$6 = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$6(o, p);
  }

  function _defineProperty$7(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  var DecisionTableHead =
  /*#__PURE__*/
  function (_Component) {
    _inherits$6(DecisionTableHead, _Component);

    function DecisionTableHead(props, context) {
      var _this;

      _classCallCheck$g(this, DecisionTableHead);

      _this = _possibleConstructorReturn$6(this, _getPrototypeOf$6(DecisionTableHead).call(this, props, context));

      _defineProperty$7(_assertThisInitialized$6(_this), "onElementsChanged", function () {
        _this.forceUpdate();
      });

      mixin(_assertThisInitialized$6(_this), ComponentWithSlots);
      _this._sheet = context.injector.get('sheet');
      _this._changeSupport = context.changeSupport;
      return _this;
    }

    _createClass$f(DecisionTableHead, [{
      key: "componentWillMount",
      value: function componentWillMount() {
        var root = this._sheet.getRoot();

        this._changeSupport.onElementsChanged(root.id, this.onElementsChanged);
      }
    }, {
      key: "componentWillUnmount",
      value: function componentWillUnmount() {
        var root = this._sheet.getRoot();

        this._changeSupport.offElementsChanged(root.id, this.onElementsChanged);
      }
    }, {
      key: "render",
      value: function render() {
        var _this2 = this;

        var root = this._sheet.getRoot();

        if (!is(root, 'dmn:DMNElement')) {
          return null;
        }

        var businessObject = getBusinessObject(root);
        var inputs = businessObject.input,
            outputs = businessObject.output;
        return createVNode(1, "thead", null, createVNode(1, "tr", null, [createVNode(1, "th", "index-column"), this.slotFills({
          type: 'cell',
          context: {
            cellType: 'before-label-cells'
          }
        }), inputs && inputs.map(function (input, index) {
          var width = input.width || '192px';
          return _this2.slotFill({
            type: 'cell',
            context: {
              cellType: 'input-header',
              input: input,
              index: index,
              inputsLength: inputs.length,
              width: width
            },
            key: input.id
          }, DefaultInputHeaderCell);
        }), outputs.map(function (output, index) {
          return _this2.slotFill({
            type: 'cell',
            context: {
              cellType: 'output-header',
              output: output,
              index: index,
              outputsLength: outputs.length
            },
            key: output.id
          }, DefaultOutputHeaderCell);
        }), this.slotFills({
          type: 'cell',
          context: {
            cellType: 'after-label-cells'
          }
        })], 0), 2);
      }
    }]);

    return DecisionTableHead;
  }(Component); // default components ///////////////////////

  function DefaultInputHeaderCell(props, context) {
    var input = props.input,
        className = props.className,
        index = props.index;
    var label = input.label,
        inputExpression = input.inputExpression,
        inputValues = input.inputValues;
    var translate = context.injector.get('translate');
    var actualClassName = (className || '') + ' input-cell';
    return createVNode(1, "th", actualClassName, [createVNode(1, "div", "clause", index === 0 ? translate('When') : translate('And'), 0), label ? createVNode(1, "div", "input-label", label, 0, {
      "title": translate('Input Label')
    }) : createVNode(1, "div", "input-expression", inputExpression.text, 0, {
      "title": translate('Input Expression')
    }), createVNode(1, "div", "input-variable", inputValues && inputValues.text || inputExpression.typeRef, 0, {
      "title": inputValues && inputValues.text ? translate('Input Values') : translate('Input Type')
    })], 0, {
      "data-col-id": input.id
    }, input.id);
  }

  function DefaultOutputHeaderCell(props, context) {
    var output = props.output,
        className = props.className,
        index = props.index;
    var label = output.label,
        name = output.name,
        outputValues = output.outputValues,
        typeRef = output.typeRef;
    var translate = context.injector.get('translate');
    var actualClassName = (className || '') + ' output-cell';
    return createVNode(1, "th", actualClassName, [createVNode(1, "div", "clause", index === 0 ? translate('Then') : translate('And'), 0), label ? createVNode(1, "div", "output-label", label, 0, {
      "title": translate('Output Label')
    }) : createVNode(1, "div", "output-name", name, 0, {
      "title": translate('Output Name')
    }), createVNode(1, "div", "output-variable", outputValues && outputValues.text || typeRef, 0, {
      "title": outputValues && outputValues.text ? translate('Output Values') : translate('Output Type')
    })], 0, null, output.id);
  }

  function DecisionTableHeadProvider(components) {
    components.onGetComponent('table.head', function () {
      return DecisionTableHead;
    });
  }
  DecisionTableHeadProvider.$inject = ['components'];

  var decisionTableHeadModule = {
    __init__: ['decisionTableHeadProvider'],
    decisionTableHeadProvider: ['type', DecisionTableHeadProvider]
  };

  function _typeof$9(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$9 = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$9 = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$9(obj);
  }

  function _classCallCheck$h(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$g(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$g(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$g(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$g(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$7(self, call) {
    if (call && (_typeof$9(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$7(self);
  }

  function _getPrototypeOf$7(o) {
    _getPrototypeOf$7 = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$7(o);
  }

  function _assertThisInitialized$7(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$7(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$7(subClass, superClass);
  }

  function _setPrototypeOf$7(o, p) {
    _setPrototypeOf$7 = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$7(o, p);
  }

  var DecisionTablePropertiesComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$7(DecisionTablePropertiesComponent, _Component);

    function DecisionTablePropertiesComponent(props, context) {
      var _this;

      _classCallCheck$h(this, DecisionTablePropertiesComponent);

      _this = _possibleConstructorReturn$7(this, _getPrototypeOf$7(DecisionTablePropertiesComponent).call(this, props, context));
      inject(_assertThisInitialized$7(_this));
      return _this;
    }

    _createClass$g(DecisionTablePropertiesComponent, [{
      key: "render",
      value: function render() {
        var root = this.sheet.getRoot();

        if (!is(root, 'dmn:DMNElement')) {
          return null;
        }

        var name = root.businessObject.$parent.name;
        var HitPolicy = this.components.getComponent('hit-policy') || NullComponent;
        return createVNode(1, "div", "decision-table-properties", [createVNode(1, "div", "decision-table-name", name, 0, {
          "title": 'Decision Name: ' + name
        }), createVNode(1, "div", "decision-table-header-separator"), createComponentVNode(2, HitPolicy)], 4);
      }
    }]);

    return DecisionTablePropertiesComponent;
  }(Component);
  DecisionTablePropertiesComponent.$inject = ['sheet', 'components'];

  function NullComponent() {
    return null;
  }

  function _classCallCheck$i(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }
  var LOW_PRIORITY$3 = 500;

  var DecisionTableProperties = function DecisionTableProperties(components) {
    _classCallCheck$i(this, DecisionTableProperties);

    components.onGetComponent('table.before', LOW_PRIORITY$3, function () {
      return DecisionTablePropertiesComponent;
    });
  };
  DecisionTableProperties.$inject = ['components'];

  var decisionTablePropertiesModule = {
    __init__: ['decisionTableProperties'],
    decisionTableProperties: ['type', DecisionTableProperties]
  };

  function _typeof$a(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$a = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$a = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$a(obj);
  }

  function _classCallCheck$j(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$h(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$h(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$h(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$h(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$8(self, call) {
    if (call && (_typeof$a(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$8(self);
  }

  function _assertThisInitialized$8(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _getPrototypeOf$8(o) {
    _getPrototypeOf$8 = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$8(o);
  }

  function _inherits$8(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$8(subClass, superClass);
  }

  function _setPrototypeOf$8(o, p) {
    _setPrototypeOf$8 = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$8(o, p);
  }

  var DecisionRulesIndexCellComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$8(DecisionRulesIndexCellComponent, _Component);

    function DecisionRulesIndexCellComponent() {
      _classCallCheck$j(this, DecisionRulesIndexCellComponent);

      return _possibleConstructorReturn$8(this, _getPrototypeOf$8(DecisionRulesIndexCellComponent).apply(this, arguments));
    }

    _createClass$h(DecisionRulesIndexCellComponent, [{
      key: "render",
      value: function render() {
        var _this$props = this.props,
            row = _this$props.row,
            rowIndex = _this$props.rowIndex;
        var components = this.context.components;
        var innerComponents = components.getComponents('cell-inner', {
          cellType: 'rule-index',
          row: row,
          rowIndex: rowIndex
        });
        return createVNode(1, "td", "rule-index", [innerComponents && innerComponents.map(function (InnerComponent) {
          return createComponentVNode(2, InnerComponent, {
            "row": row,
            "rowIndex": rowIndex
          });
        }), rowIndex + 1], 0, {
          "data-element-id": row.id,
          "data-row-id": row.id
        });
      }
    }]);

    return DecisionRulesIndexCellComponent;
  }(Component);

  function _classCallCheck$k(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var DecisionRuleIndices = function DecisionRuleIndices(components) {
    _classCallCheck$k(this, DecisionRuleIndices);

    components.onGetComponent('cell', function (_ref) {
      var cellType = _ref.cellType;

      if (cellType === 'before-rule-cells') {
        return DecisionRulesIndexCellComponent;
      }
    });
  };
  DecisionRuleIndices.$inject = ['components'];

  var decisionRuleIndicesModule = {
    __init__: ['decisionRuleIndices'],
    decisionRuleIndices: ['type', DecisionRuleIndices]
  };

  function _classCallCheck$l(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$i(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$i(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$i(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$i(Constructor, staticProps);
    return Constructor;
  }
  var EXPRESSION_LANGUAGE_OPTIONS = [{
    label: 'FEEL',
    value: 'feel'
  }, {
    label: 'JUEL',
    value: 'juel'
  }, {
    label: 'JavaScript',
    value: 'javascript'
  }, {
    label: 'Groovy',
    value: 'groovy'
  }, {
    label: 'Python',
    value: 'python'
  }, {
    label: 'JRuby',
    value: 'jruby'
  }];
  /**
   * @typedef ExpressionLanguageDescriptor
   * @property {string} value - value inserted into XML
   * @property {string} label - human-readable label
   */

  /**
   * Provide options and defaults of expression languages via config.
   *
   * @example
   *
   * // there will be two languages available with FEEL as default
   * const editor = new DmnJS({
   *   expressionLanguages: {
   *     options: [{
   *       value: 'feel',
   *       label: 'FEEL'
   *     }, {
   *       value: 'juel',
   *       label: 'JUEL'
   *     }],
   *     defaults: {
   *       editor: 'feel'
   *     }
   *   }
   * })
   */

  var ExpressionLanguages =
  /*#__PURE__*/
  function () {
    function ExpressionLanguages(injector) {
      _classCallCheck$l(this, ExpressionLanguages);

      this._injector = injector;
      var config = injector.get('config.expressionLanguages') || {};
      this._config = {
        options: EXPRESSION_LANGUAGE_OPTIONS,
        defaults: {
          editor: 'feel'
        }
      }; // first assign the list of languages as it might be required for the legacy defaults

      if (config.options) {
        this._config.options = config.options;
      }

      var legacyDefaults = this._getLegacyDefaults();

      assign(this._config.defaults, legacyDefaults, config.defaults);
    }
    /**
     * Get default expression language for a component or the editor if `componentName`
     * is not provided.
     *
     * @param {string} [componentName]
     * @returns {ExpressionLanguageDescriptor}
     */


    _createClass$i(ExpressionLanguages, [{
      key: "getDefault",
      value: function getDefault(componentName) {
        var defaults = this._config.defaults;
        var defaultFromConfig = defaults[componentName] || defaults.editor;
        return this._getLanguageByValue(defaultFromConfig) || this.getAll()[0];
      }
      /**
       * Get label for provided expression language.
       *
       * @param {string} expressionLanguageValue - value from XML
       * @returns {string}
       */

    }, {
      key: "getLabel",
      value: function getLabel(expressionLanguageValue) {
        var langauge = this._getLanguageByValue(expressionLanguageValue);

        return langauge ? langauge.label : expressionLanguageValue;
      }
      /**
       * Get list of configured expression languages.
       *
       * @returns {ExpressionLanguageDescriptor[]}
       */

    }, {
      key: "getAll",
      value: function getAll() {
        return this._config.options;
      }
    }, {
      key: "_getLegacyDefaults",
      value: function _getLegacyDefaults() {
        var defaults = {},
            injector = this._injector;
        var inputCellValue = injector.get('config.defaultInputExpressionLanguage');
        var outputCellValue = injector.get('config.defaultOutputExpressionLanguage');

        if (inputCellValue) {
          defaults.inputCell = inputCellValue;
        }

        if (outputCellValue) {
          defaults.outputCell = outputCellValue;
        }

        return defaults;
      }
    }, {
      key: "_getLanguageByValue",
      value: function _getLanguageByValue(value) {
        return find(this.getAll(), function (language) {
          return value === language.value;
        });
      }
    }]);

    return ExpressionLanguages;
  }();
  ExpressionLanguages.$inject = ['injector'];

  var ExpressionLanguagesModule = {
    __init__: ['expressionLanguages'],
    expressionLanguages: ['type', ExpressionLanguages]
  };

  function _typeof$b(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$b = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$b = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$b(obj);
  }

  function _classCallCheck$m(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$j(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$j(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$j(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$j(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$9(self, call) {
    if (call && (_typeof$b(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$9(self);
  }

  function _assertThisInitialized$9(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _getPrototypeOf$9(o) {
    _getPrototypeOf$9 = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$9(o);
  }

  function _inherits$9(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$9(subClass, superClass);
  }

  function _setPrototypeOf$9(o, p) {
    _setPrototypeOf$9 = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$9(o, p);
  }

  var DecisionRulesBodyComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$9(DecisionRulesBodyComponent, _Component);

    function DecisionRulesBodyComponent() {
      _classCallCheck$m(this, DecisionRulesBodyComponent);

      return _possibleConstructorReturn$9(this, _getPrototypeOf$9(DecisionRulesBodyComponent).apply(this, arguments));
    }

    _createClass$j(DecisionRulesBodyComponent, [{
      key: "render",
      value: function render(_ref) {
        var rows = _ref.rows,
            cols = _ref.cols;
        var components = this.context.components;
        return createVNode(1, "tbody", null, rows.map(function (row, rowIndex) {
          var RowComponent = components.getComponent('row', {
            rowType: 'rule'
          });
          return RowComponent && createComponentVNode(2, RowComponent, {
            "row": row,
            "rowIndex": rowIndex,
            "cols": cols
          }, row.id);
        }), 0);
      }
    }]);

    return DecisionRulesBodyComponent;
  }(Component);

  function _typeof$c(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$c = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$c = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$c(obj);
  }

  function _classCallCheck$n(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$k(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$k(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$k(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$k(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$a(self, call) {
    if (call && (_typeof$c(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$a(self);
  }

  function _getPrototypeOf$a(o) {
    _getPrototypeOf$a = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$a(o);
  }

  function _assertThisInitialized$a(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$a(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$a(subClass, superClass);
  }

  function _setPrototypeOf$a(o, p) {
    _setPrototypeOf$a = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$a(o, p);
  }

  var DecisionRulesRowComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$a(DecisionRulesRowComponent, _Component);

    function DecisionRulesRowComponent(props, context) {
      var _this;

      _classCallCheck$n(this, DecisionRulesRowComponent);

      _this = _possibleConstructorReturn$a(this, _getPrototypeOf$a(DecisionRulesRowComponent).call(this, props, context));
      mixin(_assertThisInitialized$a(_this), ComponentWithSlots);
      return _this;
    }

    _createClass$k(DecisionRulesRowComponent, [{
      key: "render",
      value: function render() {
        var _this2 = this;

        var _this$props = this.props,
            row = _this$props.row,
            rowIndex = _this$props.rowIndex,
            cols = _this$props.cols;
        var cells = row.cells;
        return createVNode(1, "tr", null, [this.slotFills({
          type: 'cell',
          context: {
            cellType: 'before-rule-cells',
            row: row,
            rowIndex: rowIndex
          }
        }), cells.map(function (cell, colIndex) {
          return _this2.slotFill({
            type: 'cell',
            context: {
              cellType: 'rule',
              cell: cell,
              rowIndex: rowIndex,
              colIndex: colIndex
            },
            key: cell.id,
            row: row,
            col: cols[colIndex]
          });
        }), this.slotFills({
          type: 'cell',
          context: {
            cellType: 'after-rule-cells',
            row: row,
            rowIndex: rowIndex
          }
        })], 0);
      }
    }]);

    return DecisionRulesRowComponent;
  }(Component);

  function _typeof$d(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$d = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$d = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$d(obj);
  }

  function _classCallCheck$o(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$l(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$l(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$l(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$l(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$b(self, call) {
    if (call && (_typeof$d(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$b(self);
  }

  function _assertThisInitialized$b(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _getPrototypeOf$b(o) {
    _getPrototypeOf$b = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$b(o);
  }

  function _inherits$b(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$b(subClass, superClass);
  }

  function _setPrototypeOf$b(o, p) {
    _setPrototypeOf$b = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$b(o, p);
  }

  var DecisionRulesCellComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$b(DecisionRulesCellComponent, _Component);

    function DecisionRulesCellComponent() {
      _classCallCheck$o(this, DecisionRulesCellComponent);

      return _possibleConstructorReturn$b(this, _getPrototypeOf$b(DecisionRulesCellComponent).apply(this, arguments));
    }

    _createClass$l(DecisionRulesCellComponent, [{
      key: "render",
      value: function render() {
        var _this$props = this.props,
            cell = _this$props.cell,
            row = _this$props.row,
            col = _this$props.col;

        if (is(cell, 'dmn:UnaryTests')) {
          return createComponentVNode(2, HeaderCell, {
            "className": "input-cell",
            "elementId": cell.id,
            "data-row-id": row.id,
            "data-col-id": col.id,
            children: cell.businessObject.text
          });
        } else {
          return createComponentVNode(2, HeaderCell, {
            "className": "output-cell",
            "elementId": cell.id,
            "data-row-id": row.id,
            "data-col-id": col.id,
            children: cell.businessObject.text
          });
        }
      }
    }]);

    return DecisionRulesCellComponent;
  }(Component);

  function _classCallCheck$p(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var Rules = function Rules(components) {
    _classCallCheck$p(this, Rules);

    components.onGetComponent('table.body', function () {
      return DecisionRulesBodyComponent;
    });
    components.onGetComponent('row', function (_ref) {
      var rowType = _ref.rowType;

      if (rowType === 'rule') {
        return DecisionRulesRowComponent;
      }
    });
    components.onGetComponent('cell', function (_ref2) {
      var cellType = _ref2.cellType;

      if (cellType === 'rule') {
        return DecisionRulesCellComponent;
      }
    });
  };
  Rules.$inject = ['components'];

  var decisoinRulesModule = {
    __depends__: [ExpressionLanguagesModule],
    __init__: ['decisionRules'],
    decisionRules: ['type', Rules]
  };

  /* eslint max-len: 0 */
  var HIT_POLICIES = [{
    label: 'Unique',
    value: {
      hitPolicy: 'UNIQUE',
      aggregation: undefined
    },
    explanation: 'No overlap is possible and all rules are disjoint. Only a single rule can be matched'
  }, {
    label: 'First',
    value: {
      hitPolicy: 'FIRST',
      aggregation: undefined
    },
    explanation: 'Rules may overlap. The first matching rule will be chosen'
  }, {
    label: 'Priority',
    value: {
      hitPolicy: 'PRIORITY',
      aggregation: undefined
    },
    explanation: 'Rules may overlap. The one with the highest priority will be chosen'
  }, {
    label: 'Any',
    value: {
      hitPolicy: 'ANY',
      aggregation: undefined
    },
    explanation: 'Rules may overlap. Their output have to match'
  }, {
    label: 'Collect',
    value: {
      hitPolicy: 'COLLECT',
      aggregation: undefined
    },
    explanation: 'Collects the values of all matching rules'
  }, {
    label: 'Collect (Sum)',
    value: {
      hitPolicy: 'COLLECT',
      aggregation: 'SUM'
    },
    explanation: 'Collects the values of all matching rules and sums up to a single value'
  }, {
    label: 'Collect (Min)',
    value: {
      hitPolicy: 'COLLECT',
      aggregation: 'MIN'
    },
    explanation: 'Collects the values of all matching rules and uses the lowest value'
  }, {
    label: 'Collect (Max)',
    value: {
      hitPolicy: 'COLLECT',
      aggregation: 'MAX'
    },
    explanation: 'Collects the values of all matching rules and uses the highest value'
  }, {
    label: 'Collect (Count)',
    value: {
      hitPolicy: 'COLLECT',
      aggregation: 'COUNT'
    },
    explanation: 'Collects the values of all matching rules and counts the number of them'
  }, {
    label: 'Rule order',
    value: {
      hitPolicy: 'RULE ORDER',
      aggregation: undefined
    },
    explanation: 'Collects the values of all matching rules in rule order'
  }, {
    label: 'Output order',
    value: {
      hitPolicy: 'OUTPUT ORDER',
      aggregation: undefined
    },
    explanation: 'Collects the values of all matching rules in decreasing output priority order'
  }];

  function _typeof$e(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$e = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$e = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$e(obj);
  }

  function _classCallCheck$q(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$m(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$m(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$m(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$m(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$c(self, call) {
    if (call && (_typeof$e(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$c(self);
  }

  function _getPrototypeOf$c(o) {
    _getPrototypeOf$c = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$c(o);
  }

  function _assertThisInitialized$c(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$c(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$c(subClass, superClass);
  }

  function _setPrototypeOf$c(o, p) {
    _setPrototypeOf$c = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$c(o, p);
  }

  var HitPolicy =
  /*#__PURE__*/
  function (_Component) {
    _inherits$c(HitPolicy, _Component);

    function HitPolicy(props, context) {
      var _this;

      _classCallCheck$q(this, HitPolicy);

      _this = _possibleConstructorReturn$c(this, _getPrototypeOf$c(HitPolicy).call(this, props, context));
      inject(_assertThisInitialized$c(_this));
      return _this;
    }

    _createClass$m(HitPolicy, [{
      key: "getRoot",
      value: function getRoot() {
        return this.sheet.getRoot();
      }
    }, {
      key: "render",
      value: function render() {
        var root = this.getRoot(),
            businessObject = root.businessObject;
        var aggregation = businessObject.aggregation,
            hitPolicy = businessObject.hitPolicy;
        var hitPolicyEntry = find(HIT_POLICIES, function (entry) {
          return isEqualHitPolicy(entry.value, {
            aggregation: aggregation,
            hitPolicy: hitPolicy
          });
        });
        return createVNode(1, "div", "hit-policy header", [createVNode(1, "label", "dms-label", createTextVNode("Hit Policy:"), 2), createVNode(1, "span", "hit-policy-value", hitPolicyEntry.label, 0)], 4, {
          "title": hitPolicyEntry.explanation
        });
      }
    }]);

    return HitPolicy;
  }(Component);
  HitPolicy.$inject = ['sheet']; // helpers //////////////////////

  function isEqualHitPolicy(a, b) {
    return a.hitPolicy === b.hitPolicy && a.aggregation === b.aggregation;
  }

  function HitPolicyProvider(components) {
    components.onGetComponent('hit-policy', function () {
      return HitPolicy;
    });
  }
  HitPolicyProvider.$inject = ['components'];

  var hitPolicyModule = {
    __init__: ['hitPolicyProvider'],
    hitPolicyProvider: ['type', HitPolicyProvider]
  };

  function _typeof$f(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$f = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$f = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$f(obj);
  }

  function _classCallCheck$r(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$n(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$n(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$n(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$n(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$d(self, call) {
    if (call && (_typeof$f(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$d(self);
  }

  function _getPrototypeOf$d(o) {
    _getPrototypeOf$d = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$d(o);
  }

  function _assertThisInitialized$d(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$d(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$d(subClass, superClass);
  }

  function _setPrototypeOf$d(o, p) {
    _setPrototypeOf$d = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$d(o, p);
  }

  function _defineProperty$8(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  var ViewDrdComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$d(ViewDrdComponent, _Component);

    function ViewDrdComponent(props, context) {
      var _this;

      _classCallCheck$r(this, ViewDrdComponent);

      _this = _possibleConstructorReturn$d(this, _getPrototypeOf$d(ViewDrdComponent).call(this, props, context));

      _defineProperty$8(_assertThisInitialized$d(_this), "onClick", function () {
        _this._eventBus.fire('showDrd');
      });

      var injector = context.injector;
      _this._eventBus = injector.get('eventBus');
      return _this;
    }

    _createClass$n(ViewDrdComponent, [{
      key: "render",
      value: function render() {
        var _this2 = this;

        return createVNode(1, "div", "view-drd", createVNode(1, "button", "view-drd-button", createTextVNode("View DRD"), 2, {
          "onClick": this.onClick
        }), 2, null, null, function (node) {
          return _this2.node = node;
        });
      }
    }]);

    return ViewDrdComponent;
  }(Component);

  function _classCallCheck$s(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$o(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$o(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$o(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$o(Constructor, staticProps);
    return Constructor;
  }

  var ViewDrd =
  /*#__PURE__*/
  function () {
    function ViewDrd(components, eventBus, injector, sheet) {
      var _this = this;

      _classCallCheck$s(this, ViewDrd);

      this._injector = injector;
      this._sheet = sheet;
      components.onGetComponent('table.before', function () {
        if (_this.canViewDrd()) {
          return ViewDrdComponent;
        }
      });
      eventBus.on('showDrd', function () {
        var parent = injector.get('_parent', false);
        var root = sheet.getRoot();
        var definitions = getDefinitions(root);

        if (!definitions) {
          return;
        } // open definitions


        var view = parent.getView(definitions);
        parent.open(view);
      });
    }

    _createClass$o(ViewDrd, [{
      key: "canViewDrd",
      value: function canViewDrd() {
        var parent = this._injector.get('_parent', false);

        if (!parent) {
          return false;
        }

        var root = this._sheet.getRoot();

        var definitions = getDefinitions(root);
        return !!parent.getView(definitions);
      }
    }]);

    return ViewDrd;
  }();
  ViewDrd.$inject = ['components', 'eventBus', 'injector', 'sheet']; // helpers //////////////////////

  function getDefinitions(root) {
    var businessObject = root.businessObject; // root might not have business object

    if (!businessObject) {
      return;
    }

    var decision = businessObject.$parent;
    var definitions = decision.$parent;
    return definitions;
  }

  var viewDrdModule = {
    __init__: ['viewDrd'],
    viewDrd: ['type', ViewDrd]
  };

  function Logo() {
    return createVNode(32, "svg", null, [createVNode(1, "path", null, null, 1, {
      "fill": '#000000',
      "d": 'M1.88.92v.14c0 .41-.13.68-.4.8.33.14.46.44.46.86v.33c0 .61-.33.95-.95.95H0V0h.95c.65 0 .93.3.93.92zM.63.57v1.06h.24c.24 0 .38-.1.38-.43V.98c0-.28-.1-.4-.32-.4zm0 1.63v1.22h.36c.2 0 .32-.1.32-.39v-.35c0-.37-.12-.48-.4-.48H.63zM4.18.99v.52c0 .64-.31.98-.94.98h-.3V4h-.62V0h.92c.63 0 .94.35.94.99zM2.94.57v1.35h.3c.2 0 .3-.09.3-.37v-.6c0-.29-.1-.38-.3-.38h-.3zm2.89 2.27L6.25 0h.88v4h-.6V1.12L6.1 3.99h-.6l-.46-2.82v2.82h-.55V0h.87zM8.14 1.1V4h-.56V0h.79L9 2.4V0h.56v4h-.64zm2.49 2.29v.6h-.6v-.6zM12.12 1c0-.63.33-1 .95-1 .61 0 .95.37.95 1v2.04c0 .64-.34 1-.95 1-.62 0-.95-.37-.95-1zm.62 2.08c0 .28.13.39.33.39s.32-.1.32-.4V.98c0-.29-.12-.4-.32-.4s-.33.11-.33.4z'
    }), createVNode(1, "path", null, null, 1, {
      "fill": '#000000',
      "d": 'M0 4.53h14.02v1.04H0zM11.08 0h.63v.62h-.63zm.63 4V1h-.63v2.98z'
    })], 4, {
      "xmlns": 'http://www.w3.org/2000/svg',
      "viewBox": '0 0 14.02 5.57',
      "width": '53',
      "height": '21',
      "style": 'vertical-align:middle'
    });
  }

  function _typeof$g(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$g = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$g = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$g(obj);
  }

  function _classCallCheck$t(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$p(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$p(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$p(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$p(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$e(self, call) {
    if (call && (_typeof$g(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$e(self);
  }

  function _getPrototypeOf$e(o) {
    _getPrototypeOf$e = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$e(o);
  }

  function _assertThisInitialized$e(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$e(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$e(subClass, superClass);
  }

  function _setPrototypeOf$e(o, p) {
    _setPrototypeOf$e = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$e(o, p);
  }

  function _defineProperty$9(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  var PoweredByLogoComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$e(PoweredByLogoComponent, _Component);

    function PoweredByLogoComponent(props, context) {
      var _this;

      _classCallCheck$t(this, PoweredByLogoComponent);

      _this = _possibleConstructorReturn$e(this, _getPrototypeOf$e(PoweredByLogoComponent).call(this, props, context));

      _defineProperty$9(_assertThisInitialized$e(_this), "onClick", function () {
        _this._eventBus.fire('poweredBy.show');
      });

      var injector = context.injector;
      _this._eventBus = injector.get('eventBus');
      return _this;
    }

    _createClass$p(PoweredByLogoComponent, [{
      key: "render",
      value: function render() {
        var _this2 = this;

        return createVNode(1, "div", 'powered-by', createVNode(1, "div", 'powered-by__logo', createComponentVNode(2, Logo), 2, {
          "title": 'Powered by bpmn.io',
          "onClick": this.onClick
        }, null, function (node) {
          return _this2.node = node;
        }), 2);
      }
    }]);

    return PoweredByLogoComponent;
  }(Component);

  function _typeof$h(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$h = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$h = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$h(obj);
  }

  function _classCallCheck$u(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$q(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$q(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$q(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$q(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$f(self, call) {
    if (call && (_typeof$h(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$f(self);
  }

  function _getPrototypeOf$f(o) {
    _getPrototypeOf$f = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$f(o);
  }

  function _assertThisInitialized$f(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$f(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$f(subClass, superClass);
  }

  function _setPrototypeOf$f(o, p) {
    _setPrototypeOf$f = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$f(o, p);
  }

  var PoweredByOverlayComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$f(PoweredByOverlayComponent, _Component);

    function PoweredByOverlayComponent(props) {
      var _this;

      _classCallCheck$u(this, PoweredByOverlayComponent);

      _this = _possibleConstructorReturn$f(this, _getPrototypeOf$f(PoweredByOverlayComponent).call(this, props));
      _this.state = {
        show: false
      };
      _this.onClick = _this.onClick.bind(_assertThisInitialized$f(_this));
      _this.onShow = _this.onShow.bind(_assertThisInitialized$f(_this));
      return _this;
    }

    _createClass$q(PoweredByOverlayComponent, [{
      key: "onClick",
      value: function onClick() {
        this.setState({
          show: false
        });
      }
    }, {
      key: "onShow",
      value: function onShow() {
        this.setState({
          show: true
        });
      }
    }, {
      key: "componentWillMount",
      value: function componentWillMount() {
        var eventBus = this._eventBus = this.context.injector.get('eventBus');
        eventBus.on('poweredBy.show', this.onShow);
      }
    }, {
      key: "componentWillUnmount",
      value: function componentWillUnmount() {
        this._eventBus.off('poweredBy.show', this.onShow);
      }
    }, {
      key: "render",
      value: function render() {
        var show = this.state.show;
        return show && createVNode(1, "div", "powered-by-overlay", createVNode(1, "div", "powered-by-overlay-content", [createVNode(1, "a", "logo", createComponentVNode(2, Logo), 2, {
          "href": "https://bpmn.io",
          "target": "_blank",
          "rel": "noopener"
        }), createVNode(1, "span", null, [createTextVNode("Web-based tooling for BPMN, DMN and CMMN diagrams powered by "), createVNode(1, "a", null, createTextVNode("bpmn.io"), 2, {
          "href": "http://bpmn.io",
          "target": "_blank"
        }), createTextVNode(".")], 4)], 4, {
          "onClick": function onClick(e) {
            return e.stopPropagation();
          }
        }), 2, {
          "onClick": this.onClick
        });
      }
    }]);

    return PoweredByOverlayComponent;
  }(Component);

  function _classCallCheck$v(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var PoweredBy = function PoweredBy(components, eventBus) {
    _classCallCheck$v(this, PoweredBy);

    components.onGetComponent('table.before', function () {
      return PoweredByLogoComponent;
    });
    components.onGetComponent('table.before', function () {
      return PoweredByOverlayComponent;
    });
  };
  PoweredBy.$inject = ['components', 'eventBus'];

  var PoweredByModule = {
    __init__: ['poweredBy'],
    poweredBy: ['type', PoweredBy]
  };

  function _typeof$i(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$i = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$i = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$i(obj);
  }

  function _toConsumableArray$5(arr) {
    return _arrayWithoutHoles$5(arr) || _iterableToArray$5(arr) || _nonIterableSpread$5();
  }

  function _nonIterableSpread$5() {
    throw new TypeError("Invalid attempt to spread non-iterable instance");
  }

  function _iterableToArray$5(iter) {
    if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter);
  }

  function _arrayWithoutHoles$5(arr) {
    if (Array.isArray(arr)) {
      for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) {
        arr2[i] = arr[i];
      }

      return arr2;
    }
  }

  function _objectWithoutProperties$4(source, excluded) {
    if (source == null) return {};

    var target = _objectWithoutPropertiesLoose$4(source, excluded);

    var key, i;

    if (Object.getOwnPropertySymbols) {
      var sourceSymbolKeys = Object.getOwnPropertySymbols(source);

      for (i = 0; i < sourceSymbolKeys.length; i++) {
        key = sourceSymbolKeys[i];
        if (excluded.indexOf(key) >= 0) continue;
        if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue;
        target[key] = source[key];
      }
    }

    return target;
  }

  function _objectWithoutPropertiesLoose$4(source, excluded) {
    if (source == null) return {};
    var target = {};
    var sourceKeys = Object.keys(source);
    var key, i;

    for (i = 0; i < sourceKeys.length; i++) {
      key = sourceKeys[i];
      if (excluded.indexOf(key) >= 0) continue;
      target[key] = source[key];
    }

    return target;
  }

  function _classCallCheck$w(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$r(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$r(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$r(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$r(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$g(self, call) {
    if (call && (_typeof$i(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$g(self);
  }

  function _assertThisInitialized$g(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _get(target, property, receiver) {
    if (typeof Reflect !== "undefined" && Reflect.get) {
      _get = Reflect.get;
    } else {
      _get = function _get(target, property, receiver) {
        var base = _superPropBase(target, property);

        if (!base) return;
        var desc = Object.getOwnPropertyDescriptor(base, property);

        if (desc.get) {
          return desc.get.call(receiver);
        }

        return desc.value;
      };
    }

    return _get(target, property, receiver || target);
  }

  function _superPropBase(object, property) {
    while (!Object.prototype.hasOwnProperty.call(object, property)) {
      object = _getPrototypeOf$g(object);
      if (object === null) break;
    }

    return object;
  }

  function _getPrototypeOf$g(o) {
    _getPrototypeOf$g = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$g(o);
  }

  function _inherits$g(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$g(subClass, superClass);
  }

  function _setPrototypeOf$g(o, p) {
    _setPrototypeOf$g = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$g(o, p);
  }

  var Viewer$1 =
  /*#__PURE__*/
  function (_Table) {
    _inherits$g(Viewer, _Table);

    function Viewer() {
      var _this;

      var options = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

      _classCallCheck$w(this, Viewer);

      var container = Viewer._createContainer();

      _this = _possibleConstructorReturn$g(this, _getPrototypeOf$g(Viewer).call(this, assign(options, {
        renderer: {
          container: container
        }
      })));
      _this._container = container;
      return _this;
    }

    _createClass$r(Viewer, [{
      key: "open",
      value: function open(decision, done) {
        var err; // use try/catch to not swallow synchronous exceptions
        // that may be raised during model parsing

        try {
          if (this._decision) {
            // clear existing rendered diagram
            this.clear();
          } // update decision


          this._decision = decision; // perform import

          return importDecision(this, decision, done);
        } catch (e) {
          err = e;
        } // handle synchronously thrown exception


        return done(err);
      }
      /**
       * Initialize the table, returning { modules: [], config }.
       *
       * @param  {Object} options
       *
       * @return {Object} init config
       */

    }, {
      key: "_init",
      value: function _init(options) {
        var modules = options.modules,
            additionalModules = options.additionalModules,
            config = _objectWithoutProperties$4(options, ["modules", "additionalModules"]);

        var baseModules = modules || this.getModules();
        var extraModules = additionalModules || [];
        var staticModules = [{
          decisionTable: ['value', this]
        }];
        var allModules = [PoweredByModule].concat(_toConsumableArray$5(baseModules), _toConsumableArray$5(extraModules), staticModules);
        return {
          modules: allModules,
          config: config
        };
      }
      /**
       * Register an event listener
       *
       * Remove a previously added listener via {@link #off(event, callback)}.
       *
       * @param {string} event
       * @param {number} [priority]
       * @param {Function} callback
       * @param {Object} [that]
       */

    }, {
      key: "on",
      value: function on(event, priority, callback, target) {
        return this.get('eventBus').on(event, priority, callback, target);
      }
      /**
       * De-register an event listener
       *
       * @param {string} event
       * @param {Function} callback
       */

    }, {
      key: "off",
      value: function off(event, callback) {
        this.get('eventBus').off(event, callback);
      }
      /**
       * Emit an event on the underlying {@link EventBus}
       *
       * @param  {string} type
       * @param  {Object} event
       *
       * @return {Object} event processing result (if any)
       */

    }, {
      key: "_emit",
      value: function _emit(type, event) {
        return this.get('eventBus').fire(type, event);
      }
      /**
       * Attach viewer to given parent node.
       *
       * @param  {Element} parentNode
       */

    }, {
      key: "attachTo",
      value: function attachTo(parentNode) {
        if (!parentNode) {
          throw new Error('parentNode required');
        } // ensure we detach from the
        // previous, old parent


        this.detach();
        var container = this._container;
        parentNode.appendChild(container);

        this._emit('attach', {});
      }
      /**
       * Detach viewer from parent node, if attached.
       */

    }, {
      key: "detach",
      value: function detach() {
        var container = this._container,
            parentNode = container.parentNode;

        if (!parentNode) {
          return;
        }

        this._emit('detach', {});

        remove(container);
      }
    }, {
      key: "destroy",
      value: function destroy() {
        _get(_getPrototypeOf$g(Viewer.prototype), "destroy", this).call(this);

        this.detach();
      }
    }, {
      key: "getModules",
      value: function getModules() {
        return Viewer._getModules();
      }
    }], [{
      key: "_getModules",
      value: function _getModules() {
        return [annotationsModule, coreModule, TranslateModule, decisionTableHeadModule, decisionTablePropertiesModule, decisionRuleIndicesModule, decisoinRulesModule, hitPolicyModule, viewDrdModule];
      }
    }, {
      key: "_createContainer",
      value: function _createContainer() {
        return domify('<div class="dmn-decision-table-container"></div>');
      }
    }]);

    return Viewer;
  }(Table);

  function _classCallCheck$x(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$s(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$s(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$s(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$s(Constructor, staticProps);
    return Constructor;
  }

  var ChangeSupport$1 =
  /*#__PURE__*/
  function () {
    function ChangeSupport(eventBus) {
      var _this = this;

      _classCallCheck$x(this, ChangeSupport);

      this._listeners = {};
      eventBus.on('elements.changed', function (_ref) {
        var elements = _ref.elements;

        _this.elementsChanged(elements);
      });
      eventBus.on('element.updateId', function (_ref2) {
        var element = _ref2.element,
            newId = _ref2.newId;

        _this.updateId(element.id, newId);
      });
    }

    _createClass$s(ChangeSupport, [{
      key: "elementsChanged",
      value: function elementsChanged(elements) {
        var invoked = {};
        var elementsLength = elements.length;

        for (var i = 0; i < elementsLength; i++) {
          var id = elements[i].id;

          if (invoked[id]) {
            return;
          }

          invoked[id] = true;
          var listenersLength = this._listeners[id] && this._listeners[id].length;

          if (listenersLength) {
            for (var j = 0; j < listenersLength; j++) {
              // listeners might remove themselves before they get called
              this._listeners[id][j] && this._listeners[id][j]();
            }
          }
        }
      }
    }, {
      key: "onElementsChanged",
      value: function onElementsChanged(id, listener) {
        if (!this._listeners[id]) {
          this._listeners[id] = [];
        } // avoid push for better performance


        this._listeners[id][this._listeners[id].length] = listener;
      }
    }, {
      key: "offElementsChanged",
      value: function offElementsChanged(id, listener) {
        if (!this._listeners[id]) {
          return;
        }

        if (listener) {
          var idx = this._listeners[id].indexOf(listener);

          if (idx !== -1) {
            this._listeners[id].splice(idx, 1);
          }
        } else {
          this._listeners[id].length = 0;
        }
      }
    }, {
      key: "updateId",
      value: function updateId(oldId, newId) {
        if (this._listeners[oldId]) {
          this._listeners[newId] = this._listeners[oldId];
          delete this._listeners[oldId];
        }
      }
    }]);

    return ChangeSupport;
  }();
  ChangeSupport$1.$inject = ['eventBus'];

  function _classCallCheck$y(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$t(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$t(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$t(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$t(Constructor, staticProps);
    return Constructor;
  }
  var DEFAULT_PRIORITY$2 = 1000;

  var Components$1 =
  /*#__PURE__*/
  function () {
    function Components() {
      _classCallCheck$y(this, Components);

      this._listeners = {};
    }

    _createClass$t(Components, [{
      key: "getComponent",
      value: function getComponent(type, context) {
        var listeners = this._listeners[type];

        if (!listeners) {
          return;
        }

        var component;

        for (var i = 0; i < listeners.length; i++) {
          component = listeners[i].callback(context);

          if (component) {
            break;
          }
        }

        return component;
      }
    }, {
      key: "getComponents",
      value: function getComponents(type, context) {
        var listeners = this._listeners[type];

        if (!listeners) {
          return;
        }

        var components = [];

        for (var i = 0; i < listeners.length; i++) {
          var component = listeners[i].callback(context);

          if (component) {
            components.push(component);
          }
        }

        if (!components.length) {
          return;
        }

        return components;
      }
    }, {
      key: "onGetComponent",
      value: function onGetComponent(type, priority, callback) {
        if (isFunction(priority)) {
          callback = priority;
          priority = DEFAULT_PRIORITY$2;
        }

        if (!isNumber(priority)) {
          throw new Error('priority must be a number');
        }

        var listeners = this._getListeners(type);

        var existingListener, idx;
        var newListener = {
          priority: priority,
          callback: callback
        };

        for (idx = 0; existingListener = listeners[idx]; idx++) {
          if (existingListener.priority < priority) {
            // prepend newListener at before existingListener
            listeners.splice(idx, 0, newListener);
            return;
          }
        }

        listeners.push(newListener);
      }
    }, {
      key: "offGetComponent",
      value: function offGetComponent(type, callback) {
        var listeners = this._getListeners(type);

        var listener, listenerCallback, idx;

        if (callback) {
          // move through listeners from back to front
          // and remove matching listeners
          for (idx = listeners.length - 1; listener = listeners[idx]; idx--) {
            listenerCallback = listener.callback;

            if (listenerCallback === callback) {
              listeners.splice(idx, 1);
            }
          }
        } else {
          // clear listeners
          listeners.length = 0;
        }
      }
    }, {
      key: "_getListeners",
      value: function _getListeners(type) {
        var listeners = this._listeners[type];

        if (!listeners) {
          this._listeners[type] = listeners = [];
        }

        return listeners;
      }
    }]);

    return Components;
  }();

  function _typeof$j(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$j = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$j = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$j(obj);
  }

  function _classCallCheck$z(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$u(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$u(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$u(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$u(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$h(self, call) {
    if (call && (_typeof$j(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$h(self);
  }

  function _assertThisInitialized$h(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _getPrototypeOf$h(o) {
    _getPrototypeOf$h = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$h(o);
  }

  function _inherits$h(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$h(subClass, superClass);
  }

  function _setPrototypeOf$h(o, p) {
    _setPrototypeOf$h = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$h(o, p);
  }

  var ViewerComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$h(ViewerComponent, _Component);

    function ViewerComponent(props) {
      var _this;

      _classCallCheck$z(this, ViewerComponent);

      _this = _possibleConstructorReturn$h(this, _getPrototypeOf$h(ViewerComponent).call(this, props));
      var injector = _this._injector = props.injector;
      _this._changeSupport = injector.get('changeSupport');
      _this._components = injector.get('components');
      _this._renderer = injector.get('renderer');
      return _this;
    }

    _createClass$u(ViewerComponent, [{
      key: "getChildContext",
      value: function getChildContext() {
        return {
          changeSupport: this._changeSupport,
          components: this._components,
          renderer: this._renderer,
          injector: this._injector
        };
      }
    }, {
      key: "render",
      value: function render() {
        var components = this._components.getComponents('viewer');

        return createVNode(1, "div", "viewer-container", components && components.map(function (Component, index) {
          return createComponentVNode(2, Component, null, index);
        }), 0);
      }
    }]);

    return ViewerComponent;
  }(Component);

  function _classCallCheck$A(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$v(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$v(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$v(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$v(Constructor, staticProps);
    return Constructor;
  }

  var Renderer$1 =
  /*#__PURE__*/
  function () {
    function Renderer(changeSupport, components, config, eventBus, injector) {
      _classCallCheck$A(this, Renderer);

      var container = config.container;
      this._container = container;
      eventBus.on('renderer.mount', function () {
        render(createComponentVNode(2, ViewerComponent, {
          "injector": injector
        }), container);
      });
      eventBus.on('renderer.unmount', function () {
        render(null, container);
      });
    }

    _createClass$v(Renderer, [{
      key: "getContainer",
      value: function getContainer() {
        return this._container;
      }
    }]);

    return Renderer;
  }();
  Renderer$1.$inject = ['changeSupport', 'components', 'config.renderer', 'eventBus', 'injector'];

  var core$1 = {
    __init__: ['changeSupport', 'components', 'renderer'],
    changeSupport: ['type', ChangeSupport$1],
    components: ['type', Components$1],
    eventBus: ['type', EventBus],
    renderer: ['type', Renderer$1]
  };

  function _objectWithoutProperties$5(source, excluded) {
    if (source == null) return {};

    var target = _objectWithoutPropertiesLoose$5(source, excluded);

    var key, i;

    if (Object.getOwnPropertySymbols) {
      var sourceSymbolKeys = Object.getOwnPropertySymbols(source);

      for (i = 0; i < sourceSymbolKeys.length; i++) {
        key = sourceSymbolKeys[i];
        if (excluded.indexOf(key) >= 0) continue;
        if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue;
        target[key] = source[key];
      }
    }

    return target;
  }

  function _objectWithoutPropertiesLoose$5(source, excluded) {
    if (source == null) return {};
    var target = {};
    var sourceKeys = Object.keys(source);
    var key, i;

    for (i = 0; i < sourceKeys.length; i++) {
      key = sourceKeys[i];
      if (excluded.indexOf(key) >= 0) continue;
      target[key] = source[key];
    }

    return target;
  }

  function _classCallCheck$B(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$w(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$w(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$w(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$w(Constructor, staticProps);
    return Constructor;
  }
  /**
   * A base for React-style viewers.
   */

  var Viewer$2 =
  /*#__PURE__*/
  function () {
    function Viewer() {
      var options = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

      _classCallCheck$B(this, Viewer);

      var injector = options.injector;

      if (!injector) {
        var _this$_init = this._init(options),
            modules = _this$_init.modules,
            config = _this$_init.config;

        injector = createInjector$2(config, modules);
      }

      this.get = injector.get;
      this.invoke = injector.invoke;
      this.get('eventBus').fire('viewer.init');
    }
    /**
     * Intialize and return modules and config used for creation.
     *
     * @param  {Object} options
     *
     * @return {Object} { modules=[], config }
     */


    _createClass$w(Viewer, [{
      key: "_init",
      value: function _init(options) {
        var modules = options.modules,
            config = _objectWithoutProperties$5(options, ["modules"]);

        return {
          modules: modules,
          config: config
        };
      }
      /**
       * Destroy. This results in removing the attachment from the container.
       */

    }, {
      key: "destroy",
      value: function destroy() {
        var eventBus = this.get('eventBus');
        eventBus.fire('viewer.destroy');
      }
      /**
       * Clear. Should be used to reset the state of any stateful services.
       */

    }, {
      key: "clear",
      value: function clear() {
        var eventBus = this.get('eventBus');
        eventBus.fire('viewer.clear');
      }
    }]);

    return Viewer;
  }(); // helpers //////////////////////

  function bootstrap$2(bootstrapModules) {
    var modules = [],
        components = [];

    function hasModule(m) {
      return modules.indexOf(m) >= 0;
    }

    function addModule(m) {
      modules.push(m);
    }

    function visit(m) {
      if (hasModule(m)) {
        return;
      }

      (m.__depends__ || []).forEach(visit);

      if (hasModule(m)) {
        return;
      }

      addModule(m);
      (m.__init__ || []).forEach(function (c) {
        components.push(c);
      });
    }

    bootstrapModules.forEach(visit);
    var injector = new Injector(modules);
    components.forEach(function (c) {
      try {
        // eagerly resolve component (fn or string)
        injector[typeof c === 'string' ? 'get' : 'invoke'](c);
      } catch (e) {
        console.error('Failed to instantiate component');
        console.error(e.stack);
        throw e;
      }
    });
    return injector;
  }

  function createInjector$2(config, modules) {
    var bootstrapModules = [{
      config: ['value', config]
    }, core$1].concat(modules || []);
    return bootstrap$2(bootstrapModules);
  }

  function _classCallCheck$C(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$x(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$x(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$x(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$x(Constructor, staticProps);
    return Constructor;
  }
  /**
   * A single decision element registry.
   *
   * The sole purpose of this service is to provide the necessary API
   * to serve shared components, i.e. the UpdatePropertiesHandler.
   */


  var ElementRegistry$2 =
  /*#__PURE__*/
  function () {
    function ElementRegistry(viewer, eventBus) {
      _classCallCheck$C(this, ElementRegistry);

      this._eventBus = eventBus;
      this._viewer = viewer;
    }

    _createClass$x(ElementRegistry, [{
      key: "getDecision",
      value: function getDecision() {
        return this._viewer.getDecision();
      }
    }, {
      key: "updateId",
      value: function updateId(element, newId) {
        var decision = this.getDecision();

        if (element !== decision) {
          throw new Error('element !== decision');
        }

        this._eventBus.fire('element.updateId', {
          element: element,
          newId: newId
        });

        element.id = newId;
      }
    }]);

    return ElementRegistry;
  }();
  ElementRegistry$2.$inject = ['viewer', 'eventBus'];

  var CoreModule$2 = {
    __init__: ['elementRegistry'],
    elementRegistry: ['type', ElementRegistry$2]
  };

  function _typeof$k(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$k = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$k = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$k(obj);
  }

  function _classCallCheck$D(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$y(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$y(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$y(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$y(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$i(self, call) {
    if (call && (_typeof$k(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$i(self);
  }

  function _assertThisInitialized$i(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _getPrototypeOf$i(o) {
    _getPrototypeOf$i = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$i(o);
  }

  function _inherits$i(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$i(subClass, superClass);
  }

  function _setPrototypeOf$i(o, p) {
    _setPrototypeOf$i = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$i(o, p);
  }

  var DecisionPropertiesComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$i(DecisionPropertiesComponent, _Component);

    function DecisionPropertiesComponent(props, context) {
      var _this;

      _classCallCheck$D(this, DecisionPropertiesComponent);

      _this = _possibleConstructorReturn$i(this, _getPrototypeOf$i(DecisionPropertiesComponent).call(this, props, context));
      _this._viewer = context.injector.get('viewer');
      return _this;
    }

    _createClass$y(DecisionPropertiesComponent, [{
      key: "render",
      value: function render() {
        // there is only one single element
        var _this$_viewer$getDeci = this._viewer.getDecision(),
            name = _this$_viewer$getDeci.name;

        return createVNode(1, "div", "decision-properties", createVNode(1, "h3", "decision-name", name, 0), 2);
      }
    }]);

    return DecisionPropertiesComponent;
  }(Component);

  function _classCallCheck$E(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }
  var HIGH_PRIORITY = 1500;

  var DecisionProperties = function DecisionProperties(components) {
    _classCallCheck$E(this, DecisionProperties);

    components.onGetComponent('viewer', HIGH_PRIORITY, function () {
      return DecisionPropertiesComponent;
    });
  };
  DecisionProperties.$inject = ['components'];

  var DecisionPropertiesModule = {
    __init__: ['decisionProperties'],
    decisionProperties: ['type', DecisionProperties]
  };

  function _typeof$l(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$l = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$l = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$l(obj);
  }

  function _classCallCheck$F(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$z(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$z(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$z(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$z(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$j(self, call) {
    if (call && (_typeof$l(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$j(self);
  }

  function _assertThisInitialized$j(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _getPrototypeOf$j(o) {
    _getPrototypeOf$j = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$j(o);
  }

  function _inherits$j(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$j(subClass, superClass);
  }

  function _setPrototypeOf$j(o, p) {
    _setPrototypeOf$j = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$j(o, p);
  }

  var LiteralExpressionPropertiesComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$j(LiteralExpressionPropertiesComponent, _Component);

    function LiteralExpressionPropertiesComponent(props, context) {
      var _this;

      _classCallCheck$F(this, LiteralExpressionPropertiesComponent);

      _this = _possibleConstructorReturn$j(this, _getPrototypeOf$j(LiteralExpressionPropertiesComponent).call(this, props, context));
      _this._viewer = context.injector.get('viewer');
      return _this;
    }

    _createClass$z(LiteralExpressionPropertiesComponent, [{
      key: "render",
      value: function render() {
        var _this$_viewer$getDeci = this._viewer.getDecision(),
            literalExpression = _this$_viewer$getDeci.decisionLogic,
            variable = _this$_viewer$getDeci.variable;

        return createVNode(1, "div", "literal-expression-properties", createVNode(1, "table", null, [createVNode(1, "tr", null, [createVNode(1, "td", null, createTextVNode("Variable Name:"), 2), createVNode(1, "td", null, createVNode(1, "span", null, variable.name || '-', 0), 2)], 4), createVNode(1, "tr", null, [createVNode(1, "td", null, createTextVNode("Variable Type:"), 2), createVNode(1, "td", null, createVNode(1, "span", null, variable.typeRef || '-', 0), 2)], 4), createVNode(1, "tr", null, [createVNode(1, "td", null, createTextVNode("Expression Language:"), 2), createVNode(1, "td", null, createVNode(1, "span", null, literalExpression.expressionLanguage || '-', 0), 2)], 4)], 4), 2);
      }
    }]);

    return LiteralExpressionPropertiesComponent;
  }(Component);

  function _classCallCheck$G(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }
  var LOW_PRIORITY$4 = 500;

  var DecisionProperties$1 = function DecisionProperties(components) {
    _classCallCheck$G(this, DecisionProperties);

    components.onGetComponent('viewer', LOW_PRIORITY$4, function () {
      return LiteralExpressionPropertiesComponent;
    });
  };
  DecisionProperties$1.$inject = ['components'];

  var LiteralExpressionPropertiesModule = {
    __depends__: [ExpressionLanguagesModule],
    __init__: ['literalExpressionProperties'],
    literalExpressionProperties: ['type', DecisionProperties$1]
  };

  function _typeof$m(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$m = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$m = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$m(obj);
  }

  function _classCallCheck$H(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$A(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$A(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$A(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$A(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$k(self, call) {
    if (call && (_typeof$m(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$k(self);
  }

  function _getPrototypeOf$k(o) {
    _getPrototypeOf$k = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$k(o);
  }

  function _assertThisInitialized$k(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$k(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$k(subClass, superClass);
  }

  function _setPrototypeOf$k(o, p) {
    _setPrototypeOf$k = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$k(o, p);
  }

  function _defineProperty$a(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  var PoweredByLogoComponent$1 =
  /*#__PURE__*/
  function (_Component) {
    _inherits$k(PoweredByLogoComponent, _Component);

    function PoweredByLogoComponent(props, context) {
      var _this;

      _classCallCheck$H(this, PoweredByLogoComponent);

      _this = _possibleConstructorReturn$k(this, _getPrototypeOf$k(PoweredByLogoComponent).call(this, props, context));

      _defineProperty$a(_assertThisInitialized$k(_this), "onClick", function () {
        _this._eventBus.fire('poweredBy.show');
      });

      var injector = context.injector;
      _this._eventBus = injector.get('eventBus');
      return _this;
    }

    _createClass$A(PoweredByLogoComponent, [{
      key: "render",
      value: function render() {
        var _this2 = this;

        return createVNode(1, "div", "powered-by", createVNode(1, "div", "powered-by__logo", createComponentVNode(2, Logo), 2), 2, {
          "onClick": this.onClick,
          "title": "Powered by bpmn.io"
        }, null, function (node) {
          return _this2.node = node;
        });
      }
    }]);

    return PoweredByLogoComponent;
  }(Component);

  function _typeof$n(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$n = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$n = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$n(obj);
  }

  function _classCallCheck$I(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$B(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$B(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$B(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$B(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$l(self, call) {
    if (call && (_typeof$n(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$l(self);
  }

  function _getPrototypeOf$l(o) {
    _getPrototypeOf$l = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$l(o);
  }

  function _assertThisInitialized$l(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$l(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$l(subClass, superClass);
  }

  function _setPrototypeOf$l(o, p) {
    _setPrototypeOf$l = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$l(o, p);
  }

  var PoweredByOverlayComponent$1 =
  /*#__PURE__*/
  function (_Component) {
    _inherits$l(PoweredByOverlayComponent, _Component);

    function PoweredByOverlayComponent(props) {
      var _this;

      _classCallCheck$I(this, PoweredByOverlayComponent);

      _this = _possibleConstructorReturn$l(this, _getPrototypeOf$l(PoweredByOverlayComponent).call(this, props));
      _this.state = {
        show: false
      };
      _this.onClick = _this.onClick.bind(_assertThisInitialized$l(_this));
      _this.onShow = _this.onShow.bind(_assertThisInitialized$l(_this));
      return _this;
    }

    _createClass$B(PoweredByOverlayComponent, [{
      key: "onClick",
      value: function onClick() {
        this.setState({
          show: false
        });
      }
    }, {
      key: "onShow",
      value: function onShow() {
        this.setState({
          show: true
        });
      }
    }, {
      key: "componentWillMount",
      value: function componentWillMount() {
        var eventBus = this._eventBus = this.context.injector.get('eventBus');
        eventBus.on('poweredBy.show', this.onShow);
      }
    }, {
      key: "componentWillUnmount",
      value: function componentWillUnmount() {
        this._eventBus.off('poweredBy.show', this.onShow);
      }
    }, {
      key: "render",
      value: function render() {
        var show = this.state.show;
        return show && createVNode(1, "div", "powered-by-overlay", createVNode(1, "div", "powered-by-overlay-content", [createVNode(1, "a", "logo", createComponentVNode(2, Logo), 2, {
          "href": "https://bpmn.io",
          "target": "_blank",
          "rel": "noopener"
        }), createVNode(1, "span", null, [createTextVNode("Web-based tooling for BPMN, DMN and CMMN diagrams powered by "), createVNode(1, "a", null, createTextVNode("bpmn.io"), 2, {
          "href": "http://bpmn.io",
          "target": "_blank"
        }), createTextVNode(".")], 4)], 4, {
          "onClick": function onClick(e) {
            return e.stopPropagation();
          }
        }), 2, {
          "onClick": this.onClick
        });
      }
    }]);

    return PoweredByOverlayComponent;
  }(Component);

  function _classCallCheck$J(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }
  var HIGHER_PRIORITY = 2000;

  var PoweredBy$1 = function PoweredBy(components, eventBus) {
    _classCallCheck$J(this, PoweredBy);

    components.onGetComponent('viewer', HIGHER_PRIORITY, function () {
      return PoweredByLogoComponent$1;
    });
    components.onGetComponent('viewer', function () {
      return PoweredByOverlayComponent$1;
    });
  };
  PoweredBy$1.$inject = ['components', 'eventBus'];

  var PoweredByModule$1 = {
    __init__: ['poweredBy'],
    poweredBy: ['type', PoweredBy$1]
  };

  function _typeof$o(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$o = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$o = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$o(obj);
  }

  function _classCallCheck$K(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$C(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$C(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$C(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$C(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$m(self, call) {
    if (call && (_typeof$o(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$m(self);
  }

  function _assertThisInitialized$m(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _getPrototypeOf$m(o) {
    _getPrototypeOf$m = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$m(o);
  }

  function _inherits$m(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$m(subClass, superClass);
  }

  function _setPrototypeOf$m(o, p) {
    _setPrototypeOf$m = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$m(o, p);
  }

  var TextareaComponent =
  /*#__PURE__*/
  function (_Component) {
    _inherits$m(TextareaComponent, _Component);

    function TextareaComponent(props, context) {
      var _this;

      _classCallCheck$K(this, TextareaComponent);

      _this = _possibleConstructorReturn$m(this, _getPrototypeOf$m(TextareaComponent).call(this, props, context));
      _this._viewer = context.injector.get('viewer');
      return _this;
    }

    _createClass$C(TextareaComponent, [{
      key: "render",
      value: function render() {
        var text = this._viewer.getDecision().decisionLogic.text;

        return createVNode(1, "div", "textarea", createVNode(1, "div", "content", text, 0), 2);
      }
    }]);

    return TextareaComponent;
  }(Component);

  function _classCallCheck$L(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  var Textarea = function Textarea(components) {
    _classCallCheck$L(this, Textarea);

    components.onGetComponent('viewer', function () {
      return TextareaComponent;
    });
  };
  Textarea.$inject = ['components'];

  var TextareaModule = {
    __init__: ['textarea'],
    textarea: ['type', Textarea]
  };

  function _typeof$p(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$p = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$p = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$p(obj);
  }

  function _classCallCheck$M(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$D(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$D(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$D(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$D(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$n(self, call) {
    if (call && (_typeof$p(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$n(self);
  }

  function _getPrototypeOf$n(o) {
    _getPrototypeOf$n = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$n(o);
  }

  function _assertThisInitialized$n(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _inherits$n(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$n(subClass, superClass);
  }

  function _setPrototypeOf$n(o, p) {
    _setPrototypeOf$n = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$n(o, p);
  }

  function _defineProperty$b(obj, key, value) {
    if (key in obj) {
      Object.defineProperty(obj, key, {
        value: value,
        enumerable: true,
        configurable: true,
        writable: true
      });
    } else {
      obj[key] = value;
    }

    return obj;
  }

  var ViewDrdComponent$1 =
  /*#__PURE__*/
  function (_Component) {
    _inherits$n(ViewDrdComponent, _Component);

    function ViewDrdComponent(props, context) {
      var _this;

      _classCallCheck$M(this, ViewDrdComponent);

      _this = _possibleConstructorReturn$n(this, _getPrototypeOf$n(ViewDrdComponent).call(this, props, context));

      _defineProperty$b(_assertThisInitialized$n(_this), "onClick", function () {
        _this._eventBus.fire('showDrd');
      });

      var injector = context.injector;
      _this._eventBus = injector.get('eventBus');
      return _this;
    }

    _createClass$D(ViewDrdComponent, [{
      key: "render",
      value: function render() {
        var _this2 = this;

        return createVNode(1, "div", "view-drd", createVNode(1, "button", "view-drd-button", createTextVNode("View DRD"), 2, {
          "onClick": this.onClick
        }), 2, null, null, function (node) {
          return _this2.node = node;
        });
      }
    }]);

    return ViewDrdComponent;
  }(Component);

  function _classCallCheck$N(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$E(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$E(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$E(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$E(Constructor, staticProps);
    return Constructor;
  }
  var VERY_HIGH_PRIORITY = 2000;

  var ViewDrd$1 =
  /*#__PURE__*/
  function () {
    function ViewDrd(components, viewer, eventBus, injector) {
      var _this = this;

      _classCallCheck$N(this, ViewDrd);

      this._injector = injector;
      this._viewer = viewer;
      components.onGetComponent('viewer', VERY_HIGH_PRIORITY, function () {
        if (_this.canViewDrd()) {
          return ViewDrdComponent$1;
        }
      });
      eventBus.on('showDrd', function () {
        var parent = injector.get('_parent', false); // there is only one single element

        var definitions = _this.getDefinitions(); // open definitions


        var view = parent.getView(definitions);
        parent.open(view);
      });
    }

    _createClass$E(ViewDrd, [{
      key: "canViewDrd",
      value: function canViewDrd() {
        var parent = this._injector.get('_parent', false);

        if (!parent) {
          return;
        } // there is only one single element


        var definitions = this.getDefinitions();
        return !!parent.getView(definitions);
      }
    }, {
      key: "getDefinitions",
      value: function getDefinitions() {
        return _getDefinitions(this._viewer.getDecision());
      }
    }]);

    return ViewDrd;
  }();
  ViewDrd$1.$inject = ['components', 'viewer', 'eventBus', 'injector']; // helpers //////////////////////

  function _getDefinitions(decision) {
    var definitions = decision.$parent;
    return definitions;
  }

  var ViewDrdModule = {
    __init__: ['viewDrd'],
    viewDrd: ['type', ViewDrd$1]
  };

  function _typeof$q(obj) {
    if (typeof Symbol === "function" && _typeof(Symbol.iterator) === "symbol") {
      _typeof$q = function _typeof$1(obj) {
        return _typeof(obj);
      };
    } else {
      _typeof$q = function _typeof$1(obj) {
        return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof(obj);
      };
    }

    return _typeof$q(obj);
  }

  function _toConsumableArray$6(arr) {
    return _arrayWithoutHoles$6(arr) || _iterableToArray$6(arr) || _nonIterableSpread$6();
  }

  function _nonIterableSpread$6() {
    throw new TypeError("Invalid attempt to spread non-iterable instance");
  }

  function _iterableToArray$6(iter) {
    if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter);
  }

  function _arrayWithoutHoles$6(arr) {
    if (Array.isArray(arr)) {
      for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) {
        arr2[i] = arr[i];
      }

      return arr2;
    }
  }

  function _objectWithoutProperties$6(source, excluded) {
    if (source == null) return {};

    var target = _objectWithoutPropertiesLoose$6(source, excluded);

    var key, i;

    if (Object.getOwnPropertySymbols) {
      var sourceSymbolKeys = Object.getOwnPropertySymbols(source);

      for (i = 0; i < sourceSymbolKeys.length; i++) {
        key = sourceSymbolKeys[i];
        if (excluded.indexOf(key) >= 0) continue;
        if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue;
        target[key] = source[key];
      }
    }

    return target;
  }

  function _objectWithoutPropertiesLoose$6(source, excluded) {
    if (source == null) return {};
    var target = {};
    var sourceKeys = Object.keys(source);
    var key, i;

    for (i = 0; i < sourceKeys.length; i++) {
      key = sourceKeys[i];
      if (excluded.indexOf(key) >= 0) continue;
      target[key] = source[key];
    }

    return target;
  }

  function _classCallCheck$O(instance, Constructor) {
    if (!(instance instanceof Constructor)) {
      throw new TypeError("Cannot call a class as a function");
    }
  }

  function _defineProperties$F(target, props) {
    for (var i = 0; i < props.length; i++) {
      var descriptor = props[i];
      descriptor.enumerable = descriptor.enumerable || false;
      descriptor.configurable = true;
      if ("value" in descriptor) descriptor.writable = true;
      Object.defineProperty(target, descriptor.key, descriptor);
    }
  }

  function _createClass$F(Constructor, protoProps, staticProps) {
    if (protoProps) _defineProperties$F(Constructor.prototype, protoProps);
    if (staticProps) _defineProperties$F(Constructor, staticProps);
    return Constructor;
  }

  function _possibleConstructorReturn$o(self, call) {
    if (call && (_typeof$q(call) === "object" || typeof call === "function")) {
      return call;
    }

    return _assertThisInitialized$o(self);
  }

  function _assertThisInitialized$o(self) {
    if (self === void 0) {
      throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
    }

    return self;
  }

  function _get$1(target, property, receiver) {
    if (typeof Reflect !== "undefined" && Reflect.get) {
      _get$1 = Reflect.get;
    } else {
      _get$1 = function _get(target, property, receiver) {
        var base = _superPropBase$1(target, property);

        if (!base) return;
        var desc = Object.getOwnPropertyDescriptor(base, property);

        if (desc.get) {
          return desc.get.call(receiver);
        }

        return desc.value;
      };
    }

    return _get$1(target, property, receiver || target);
  }

  function _superPropBase$1(object, property) {
    while (!Object.prototype.hasOwnProperty.call(object, property)) {
      object = _getPrototypeOf$o(object);
      if (object === null) break;
    }

    return object;
  }

  function _getPrototypeOf$o(o) {
    _getPrototypeOf$o = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
      return o.__proto__ || Object.getPrototypeOf(o);
    };
    return _getPrototypeOf$o(o);
  }

  function _inherits$o(subClass, superClass) {
    if (typeof superClass !== "function" && superClass !== null) {
      throw new TypeError("Super expression must either be null or a function");
    }

    subClass.prototype = Object.create(superClass && superClass.prototype, {
      constructor: {
        value: subClass,
        writable: true,
        configurable: true
      }
    });
    if (superClass) _setPrototypeOf$o(subClass, superClass);
  }

  function _setPrototypeOf$o(o, p) {
    _setPrototypeOf$o = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
      o.__proto__ = p;
      return o;
    };

    return _setPrototypeOf$o(o, p);
  }

  var Viewer$3 =
  /*#__PURE__*/
  function (_BaseViewer) {
    _inherits$o(Viewer, _BaseViewer);

    function Viewer() {
      var _this;

      var options = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

      _classCallCheck$O(this, Viewer);

      var container = Viewer._createContainer();

      _this = _possibleConstructorReturn$o(this, _getPrototypeOf$o(Viewer).call(this, assign(options, {
        renderer: {
          container: container
        }
      })));
      _this._container = container;
      return _this;
    }

    _createClass$F(Viewer, [{
      key: "open",
      value: function open(decision, done) {
        var err; // use try/catch to not swallow synchronous exceptions
        // that may be raised during model parsing

        try {
          if (this._decision) {
            // clear existing literal expression
            this.clear(); // unmount first

            this.get('eventBus').fire('renderer.unmount');
          } // update literal expression


          this._decision = decision; // let others know about import

          this.get('eventBus').fire('import', decision);
          this.get('eventBus').fire('renderer.mount');
        } catch (e) {
          err = e;
        } // handle synchronously thrown exception


        return done(err);
      }
      /**
       * Initialize the literal expression, returning { modules: [], config }.
       *
       * @param  {Object} options
       *
       * @return {Object} init config
       */

    }, {
      key: "_init",
      value: function _init(options) {
        var modules = options.modules,
            additionalModules = options.additionalModules,
            config = _objectWithoutProperties$6(options, ["modules", "additionalModules"]);

        var baseModules = modules || this.getModules();
        var extraModules = additionalModules || [];
        var staticModules = [{
          viewer: ['value', this]
        }];
        var allModules = [].concat(_toConsumableArray$6(baseModules), _toConsumableArray$6(extraModules), staticModules);
        return {
          modules: allModules,
          config: config
        };
      }
      /**
       * Register an event listener
       *
       * Remove a previously added listener via {@link #off(event, callback)}.
       *
       * @param {string} event
       * @param {number} [priority]
       * @param {Function} callback
       * @param {Object} [that]
       */

    }, {
      key: "on",
      value: function on(event, priority, callback, target) {
        return this.get('eventBus').on(event, priority, callback, target);
      }
      /**
       * De-register an event listener
       *
       * @param {string} event
       * @param {Function} callback
       */

    }, {
      key: "off",
      value: function off(event, callback) {
        this.get('eventBus').off(event, callback);
      }
      /**
       * Emit an event on the underlying {@link EventBus}
       *
       * @param  {string} type
       * @param  {Object} event
       *
       * @return {Object} event processing result (if any)
       */

    }, {
      key: "_emit",
      value: function _emit(type, event) {
        return this.get('eventBus').fire(type, event);
      }
      /**
       * Returns the currently displayed decision.
       *
       * @return {ModdleElement}
       */

    }, {
      key: "getDecision",
      value: function getDecision() {
        return this._decision;
      }
      /**
       * Attach viewer to given parent node.
       *
       * @param  {Element} parentNode
       */

    }, {
      key: "attachTo",
      value: function attachTo(parentNode) {
        if (!parentNode) {
          throw new Error('parentNode required');
        } // ensure we detach from the
        // previous, old parent


        this.detach();
        parentNode.appendChild(this._container);

        this._emit('attach', {});
      }
      /**
       * Detach viewer from parent node, if attached.
       */

    }, {
      key: "detach",
      value: function detach() {
        var container = this._container,
            parentNode = container.parentNode;

        if (!parentNode) {
          return;
        }

        this._emit('detach', {});

        remove(container);
      }
    }, {
      key: "destroy",
      value: function destroy() {
        _get$1(_getPrototypeOf$o(Viewer.prototype), "destroy", this).call(this);

        this.detach();
      }
    }, {
      key: "getModules",
      value: function getModules() {
        return Viewer._getModules();
      }
    }], [{
      key: "_getModules",
      value: function _getModules() {
        return [CoreModule$2, DecisionPropertiesModule, LiteralExpressionPropertiesModule, PoweredByModule$1, TextareaModule, ViewDrdModule];
      }
    }, {
      key: "_createContainer",
      value: function _createContainer() {
        return domify('<div class="dmn-literal-expression-container"></div>');
      }
    }]);

    return Viewer;
  }(Viewer$2);

  /**
   * Does the definitions element contain graphical information?
   *
   * @param  {ModdleElement} definitions
   *
   * @return {boolean} true, if the definitions contains graphical information
   */
  function containsDi(definitions) {
    return definitions.dmnDI && definitions.dmnDI.diagrams && definitions.dmnDI.diagrams[0];
  }

  /**
   * The dmn viewer.
   */

  var Viewer$4 =
  /*#__PURE__*/
  function (_Manager) {
    _inherits(Viewer$2, _Manager);

    function Viewer$2() {
      _classCallCheck(this, Viewer$2);

      return _possibleConstructorReturn(this, _getPrototypeOf(Viewer$2).apply(this, arguments));
    }

    _createClass(Viewer$2, [{
      key: "_getViewProviders",
      value: function _getViewProviders() {
        return [{
          id: 'drd',
          constructor: Viewer,
          opens: function opens(element) {
            return is(element, 'dmn:Definitions') && containsDi(element);
          }
        }, {
          id: 'decisionTable',
          constructor: Viewer$1,
          opens: function opens(element) {
            return is(element, 'dmn:Decision') && is(element.decisionLogic, 'dmn:DecisionTable');
          }
        }, {
          id: 'literalExpression',
          constructor: Viewer$3,
          opens: function opens(element) {
            return is(element, 'dmn:Decision') && is(element.decisionLogic, 'dmn:LiteralExpression');
          }
        }];
      }
    }]);

    return Viewer$2;
  }(Manager);

  return Viewer$4;

})));
