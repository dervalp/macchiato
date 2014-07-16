Sitecore.component({
    name: "Select",
    add: function (options) {
        var self = this;

        if (options.length) {
            options.forEach(function (p) {
                self.Options.push(p);
            });
        } else {
            this.Options.push(options);
        }
    }
});