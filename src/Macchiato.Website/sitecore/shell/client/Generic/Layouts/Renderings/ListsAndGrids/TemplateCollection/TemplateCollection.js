Sitecore.component(["scSpeakObservableArray"], {
    name: "TemplateCollection",
    initialize: function () {
        this.data = new SpeakObservableArray([]);
        this.prepare = false;
    },
    add: function (value) {
        var component = this;
        if (value.length) {
            value.forEach(function (v) {
                component.data.push(v);
            });
        } else {
            this.data.push(value);
        }
    },
    _prepare: function () {
        if (!this.prepare) {
            var temp = document.createElement(this.TagName || "div" );
            this.el.parentNode.insertBefore(temp, this.el);
            this.el = temp;
            this.prepare = true;
        }
    },
    _renderSingle: function (value) {
        var component = this,
          componentScript = component.componentScript,
          componentType = component.componentType;

        this._prepare();

        if (!componentScript && !componentType) {
            component._s.template.get(component.template, function (tmplContent) {
                var compiled = component._s.tmpl.compile(tmplContent),
                  div = document.createElement("div"),
                  child = div.children[0];
                div.innerHTML = (component.el.innerHTML + compiled(value));
                value._s = component._s;

                sitecore.module("binding").observable(value);
                sitecore.module("koAdapter").buildViewModel(value);

                component.ko.applyBindings(value.viewModel, child);
                component.el.appendChild(child);
            });
        } else {

            var subComp = {
                parent: component,
                key: componentType,
                template: component.childTemplate,
                hasTemplate: true,
                el: document.createElement("div"),
                appendTo: component.el
            };

            component._s.utils.initializeProperties(subComp, value);

            require([
              [componentScript]
            ], function () {
                var subComponentIntantiated = sitecore.exposeComponent(subComp, component.app);
                component.children.push(subComponentIntantiated);
                subComponentIntantiated.render();
            });
        }
    },
    render: function (callback) {
        this.data.forEach(this._renderSingle.bind(this));
        this.data.on("add", this._renderSingle.bind(this));
        if (callback) {
            callback();
        }
    }
});