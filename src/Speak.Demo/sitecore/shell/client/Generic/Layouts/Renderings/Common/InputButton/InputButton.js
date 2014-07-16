Sitecore.component(["scPipeline"], {
    name: "InputButton",
    handler: function () {
        var context, invocation = this.click;

        if (this.click) {
            //Sitecore.Helpers.invocation.execute(this.click, { control: this, app: this.app });
            var i = invocation.indexOf(":");
            if (i <= 0) {
                throw "Invocation is malformed (missing 'handler:')";
            }

            context = {
                control: this,
                app: this.app,
                handler: invocation.substr(0, i),
                target: invocation.substr(i + 1)
            };

            sitecore.module("pipelines").get("Invoke").execute(context);
        }
    }
});
