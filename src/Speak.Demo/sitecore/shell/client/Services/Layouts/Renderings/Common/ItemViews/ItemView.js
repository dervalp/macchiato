Sitecore.component( {
  name: "ItemView",
  initialize: function () {
    this.ItemID = "";
    this.ItemName = "";
    this.ItemPath = "";
    this.ParentID = "";
    this.TemplateID = "";
    this.TemplateName = "";
    this.Item = "";
  },
  initialized: function () {
    this.on("change:Item", function ( ) {
      this.ItemID = this.Item.ItemID;
      this.ItemName = this.Item.ItemName;
      this.ItemPath = this.Item.ItemPath;
      this.ParentID = this.Item.ParentID;
      this.TemplateID = this.Item.TemplateID;
      this.TemplateName = this.Item.TemplateName;
    });
  }
});