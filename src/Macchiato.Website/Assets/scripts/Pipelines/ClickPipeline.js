/*
The Pipeline Part of SPEAK Framework
------------------------------------

What is exposed:222222
Sitecore.Pipelines
- run(options)
- add
- remove
- length
*/

var invocationHelper = {
    execute: function (invocation, options) {
        if (!invocation) {
            return;
        }

        var i = invocation.indexOf(":");
        if (i <= 0) {
            throw "Invocation is malformed (missing 'handler:')";
        }

        options = options || {};
        var handler = invocation.substr(0, i);
        var target = invocation.substr(i + 1);

        var context = _.extend({}, {
            handler: handler,
            target: target
        }, options);

        _sc.Pipelines.Invoke.execute(context);
    }
};

var ppl = Sitecore.Pipelines = function () {
    var pipelines = [];

    return {
        add: function (pipeline) {
            if (!pipeline || !pipeline.name || !_.isObject(pipeline)) {
                throw new "invalid pipeline";
            }

            pipelines.push(pipeline);
            this[pipeline.name] = pipeline;
        },
        remove: function (pipelineName) {
            pipelines = _.reject(pipelines, function (p) {
                return p.name === pipelineName;
            });

            delete Sitecore.Pipelines[pipelineName];
        },
        length: function () {
            return pipelines.length;
        }
    };
}();

ppl.Pipeline = function (name) {
    var result = {
        name: name,

        processors: [],

        add: function (processor) {
            if (!processor || !processor.priority || !processor.execute || !_.isNumber(processor.priority) || !_.isFunction(processor.execute)) {
                throw "not valid step";
            }

            this.processors.push(processor);
        },
        length: function () {
            return this.processors.length;
        },
        remove: function (processor) {
            this.processors = _.reject(this.processors, function (p) {
                return p === processor;
            });
        },
        execute: function (context) {
            //TODO: sort on adding processors
            var list = _.sortBy(this.processors, function (processor) {
                return processor.priority;
            });

            _.each(list, function (processor) {
                if (context.aborted) {
                    return false;
                }
                processor.execute(context);
            });
        }
    };

    return result;
};

var executeContext = function (target, context) {
    //First we check if you want to existing something in the app.
    var targets = target.split(".");
    var firstPath = targets[0];
    if (firstPath === "this") {
        new Function(target).call(context.control.model);
    } else if (context.app && firstPath === "app") {
        var ex = target.replace("app", "this");
        new Function(ex).call(context.app);
    } else {
        /*!!! dangerous zone !!!*/
        new Function(target)();
    }
}

var handleJavaScript = {
    priority: 1000,
    execute: function (context) {
        if (context.handler === "javascript") {
            if (context.target.indexOf(";") > 0) {
                _.each(context.target.split(";"), function (tar) {
                    executeContext(tar, context);
                });
            } else {
                executeContext(context.target, context);
            }
        }
    }
};

var handleCommand = {
    priority: 2000,
    execute: function (context) {
        if (context.handler === "command") {
            Sitecore.executeCommand(context.target);
        }
    }
};

var serverClick = {
    priority: 3000,
    execute: function (context) {
        if (context.handler !== "serverclick") {
            return;
        }

        //TODO: maybe we should validate
        var options = {
            url: context.target,
            type: "POST",
            dataType: "json"
        };

        var completed = function (result) {
            //TODO: validate result
            Sitecore.Pipelines.ServerInvoke.execute({
                data: result,
                model: context.model
            });
        };

        $.ajax(options).done(completed);
    }
};

var triggerEvent = {
    priority: 4000,
    execute: function (context) {
        if (context.handler !== "trigger") {
            return;
        }

        var app = context.app;
        if (!app) {
            throw "An application is a required when triggering events";
        }

        var target = context.target;
        var args = {};

        var n = target.indexOf("(");
        if (n >= 0) {
            if (target.indexOf(")", target.length - 1) == -1) {
                throw "Missing ')'";
            }
            var parameters = target.substr(n + 1, target.length - n - 2);
            args = $.parseJSON(parameters);
            target = target.substr(0, n);
        }

        args.sender = context.control;

        app.trigger(target, args);
    }
};

var updateModel = {
    priority: 1000,
    execute: function (context) {
        var viewModel = context.data.ViewModel;
        if (viewModel != null) {
            ko.mapping.fromJS(viewModel, {}, context.model);
        }
    }
};

var ivk = new Sitecore.Pipelines.Pipeline("Invoke");

ivk.add(handleJavaScript);
ivk.add(handleCommand);
ivk.add(serverClick);
ivk.add(triggerEvent);

Sitecore.Pipelines.add(ivk);

var srvppl = new Sitecore.Pipelines.Pipeline("ServerInvoke");

srvppl.add(updateModel);
Sitecore.Pipelines.add(srvppl);