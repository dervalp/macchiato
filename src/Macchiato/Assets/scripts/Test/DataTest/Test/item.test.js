describe( "Item Service", function () {

  var test1Item;

  var deleteTestItems = function ( done ) {

    var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
    } );

    async.waterfall( [

      function authenticate( callback ) {


          ItemService.superagent.post("https://" + window.location.host + "/sitecore/api/ssc/auth/login")
                    .accept("json")
                    .type("json")
                    .send({
                        domain: "sitecore",
                        username: "admin",
                        password: "b"
                    })
                    .end(function () {
                        callback();
                    });

//        ItemService.superagent.post("/sitecore/api/ssc/auth/login")
//          .accept( "json" )
//          .type( "json" )
//          .send( {
//            domain: "sitecore",
//            username: "admin",
//            password: "b"
//          } )
//          .end( function () {
//            callback();
//          } );

      },

      function fetchItems( callback ) {

        var masterSitecoreContentHomeId = "110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9",
          itemsToDelete = [];

        itemService.fetchItem( masterSitecoreContentHomeId ).execute().then( function ( homeItem ) {

          homeItem.fetchChildren().database( "master" ).execute().then( function ( homeItemChildren ) {

            homeItemChildren.forEach( function ( homeChild ) {

              if ( /^ItemService Test Item/i.test( homeChild.ItemName ) ) {
                itemsToDelete.push( homeChild );
              }

            } );

            callback( null, itemsToDelete );

          } ).fail( callback );

        } ).fail( callback );

      },

      function deleteItems( itemsToDelete, callback ) {

        if ( Array.isArray( itemsToDelete ) && itemsToDelete.length === 0 ) {
          return callback();
        }

        var q = async.queue( function ( itemToDelete, qCallback ) {

          itemToDelete.destroy().database( "master" ).execute().then( function () {
            qCallback();
          } ).fail( qCallback );

        }, 2 );

        q.drain = function () {
          callback();
        };

        itemsToDelete.forEach( function ( itemToDelete ) {
          q.push( itemToDelete );
        } );

      }

    ], function ( error ) {
      done( error );
    } );

  };

  before( function ( done ) {
    this.timeout( 10000 );
    this.slow( 10000 );
    deleteTestItems( done );
  } );

  // after( function ( done ) {
  //   this.timeout( 10000 );
  //   this.slow( 10000 );
  //   deleteTestItems( done );
  // } );

  it( "should exist", function () {
    ItemService.should.exist;
  } );

  it( "should fetch an item", function ( done ) {

    this.timeout( 5000 );
    this.slow( 3000 );

    var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
    } );

    var itemId = "C19E9164-FF99-4A05-B8C0-E9C931DA111F";

    itemService.fetchItem( itemId ).execute().then( function ( item ) {

      item.should.have.a.property( "ItemID", itemId.toLowerCase() );
      item.should.have.a.property( "ItemID" );
      item.should.have.a.property( "ItemName" );
      item.should.have.a.property( "ItemPath" );
      item.should.have.a.property( "ParentID" );
      item.should.have.a.property( "TemplateID" );
      item.should.have.a.property( "TemplateName" );
      item.should.have.a.property( "CloneSource" );
      item.should.have.a.property( "Keywords" );
      item.should.have.a.property( "Dimensions" );
      item.should.have.a.property( "Mime Type" );
      item.should.have.a.property( "Alt" );
      item.should.have.a.property( "File Path" );
      item.should.have.a.property( "Blob" );
      item.should.have.a.property( "Width" );
      item.should.have.a.property( "Extension" );
      item.should.have.a.property( "Size" );
      item.should.have.a.property( "Title" );
      item.should.have.a.property( "Height" );
      item.should.have.a.property( "Format" );
      item.should.have.a.property( "Description" );

      done();

    } ).fail( done );

  } );

  it( "should fetch item children", function ( done ) {

    this.timeout( 5000 );
    this.slow( 3000 );

    var itemService = new ItemService({
      url: "/sitecore/api/ssc/item"
    } );

    itemService.fetchItem( "76dbbdde-9a96-414c-846f-36d7fd8bfdc3" ).execute().then( function ( item ) {

      item.fetchChildren().execute().then( function ( children ) {

        children.length.should.be.greaterThan( 0 );
        done();

      } ).fail( done );

    } ).fail( done );

  } );

  describe( "query", function () {

    it( "should return results from a query", function ( done ) {

      this.timeout( 5000 );
      this.slow( 3000 );

      var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
      } );

      itemService.query( "6179CE7D-7C5F-4679-8220-F8E401796FD0" ).parameter( "database", "master").execute().then(function (result) {

        result.should.have.a.property( "Links" ).and.be.an.Array;
        result.should.have.a.property( "Results" ).and.be.an.Array;
        result.should.have.a.property( "TotalCount" ).and.be.a.Number;
        result.should.have.a.property( "TotalPage" ).and.be.a.Number;
        done();

      } ).fail( done );

    } );

    it( "should return results if nothing was found", function ( done ) {

      this.timeout( 5000 );
      this.slow( 3000 );

      var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
      } );

      itemService.query("B72F3363-332D-42B5-9021-08A7AD07424B").parameter("database", "master").execute().then(function (result) {

        result.TotalCount.should.equal( 0 );
        done();

      } ).fail( done );

    } );

    it( "should return a 400 if a query definition item cannot be found", function ( done ) {

      this.timeout( 5000 );
      this.slow( 3000 );

      var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
      } );

      itemService.query( "B72F3363-332D-42B5-9021-08A7AD07424F" ).execute().then(function (result) {
        done( new Error( "Nothing was found, so this promise should have failed" ) );
      } ).fail( function ( error ) {
        done();
      } );

    } );

  } );

  describe( "search", function () {

    it( "should return results", function ( done ) {

      this.timeout( 5000 );
      this.slow( 3000 );

      var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
      } );

      itemService.search( "macchiato" ).execute().then( function ( result ) {

        result.should.have.a.property( "Links" ).and.be.an.Array;
        result.should.have.a.property( "Results" ).and.be.an.Array;
        result.should.have.a.property( "TotalCount" ).and.be.a.Number;
        result.should.have.a.property( "TotalPage" ).and.be.a.Number;
        done();

      } ).fail( done );

    } );

    it( "should return results if nothing was found", function ( done ) {

      this.timeout( 5000 );
      this.slow( 3000 );

      var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
      } );

      itemService.search( "obscurity" ).execute().then( function ( result ) {

        result.TotalCount.should.equal( 0 );
        done();

      } ).fail( done );

    } );

    it( "should return a 404 if a search term was missing", function ( done ) {

      this.timeout( 5000 );
      this.slow( 3000 );

      var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
      } );

      itemService.search().execute().then( function ( result ) {
        done( new Error( "Nothing was found, so this promise should have failed" ) );
      } ).fail( function ( error ) {
        done();
      } );

    } );

    describe( "page", function () {

      it( "should return page 9", function ( done ) {

        this.timeout( 5000 );
        this.slow( 3000 );

        var itemService = new ItemService({
          url: "/sitecore/api/ssc/item"
        } );

        itemService.search( "item" ).page( 9 ).execute().then( function ( result ) {

          result.Results.length.should.be.greaterThan( 0 );
          done();

        } ).fail( done );

      } );

      it( "should return a page that does not exist", function ( done ) {

        this.timeout( 5000 );
        this.slow( 3000 );

        var itemService = new ItemService({
          url: "/sitecore/api/ssc/item"
        } );

        itemService.search( "item" ).page( 99999 ).execute().then( function ( result ) {

          result.Results.length.should.equal( 0 );
          done();

        } ).fail( done );

      } );

    } );

    describe( "sort", function () {

      it( "should return results with a custom sort", function ( done ) {

        this.timeout( 5000 );
        this.slow( 3000 );

        var itemService = new ItemService({
          url: "/sitecore/api/ssc/item"
        } );

        itemService.search( "item" ).sort( "aItemName" ).execute().then( function ( result ) {

          result.Results.length.should.be.greaterThan( 0 );
          done();

        } ).fail( done );

      } );

    } );

    describe( "take", function () {

      it( "should only return 5 results per page", function ( done ) {

        this.timeout( 5000 );
        this.slow( 3000 );

        var itemService = new ItemService({
          url: "/sitecore/api/ssc/item"
        } );

        itemService.search( "item" ).take( 5 ).execute().then( function ( result ) {

          result.Results.length.should.equal( 5 );
          done();

        } ).fail( done );

      } );

    } );

  } );

  describe( "create", function () {

    it( "should create an item", function ( done ) {

      this.timeout( 5000 );
      this.slow( 3000 );

      var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
      } );

      itemService.create( {
        TemplateID: "76036f5e-cbce-46d1-af0a-4143f9b557aa",
        ItemName: "ItemService Test Item 1",
        Title: "Is Chicken Tasty?",
        Text: "<h1>It sure is!!</h1>"
      } ).path( "/sitecore/content/home" ).execute().then( function ( testItem ) {

        testItem.ItemName.should.eql( "ItemService Test Item 1" );
        testItem.TemplateID.should.eql( "76036f5e-cbce-46d1-af0a-4143f9b557aa" );
        testItem.ItemID.should.not.be.empty.and.be.a.String;
        test1Item = testItem;

        done();

      } ).fail( done );

    } );

  } );

  describe( "save", function () {

    this.timeout( 10000 );
    this.slow( 6000 );

    it( "should save an item", function ( done ) {

      var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
      } );

      var itemId = test1Item.ItemID;

      itemService.fetchItem( itemId ).option( "trackable", true ).database( "master" ).execute().then( function ( test1Item ) {

        test1Item.once( "save", function ( error ) {

          if ( error ) {
            return done();
          }

          itemService.fetchItem( itemId ).database( "master" ).execute().then( function ( updatedTest1Item ) {

            updatedTest1Item.Title.should.eql( "Chicken is *really* tasty!" );
            done();

          } ).fail( done );

        } );

        test1Item.option( "database", "master" );
        test1Item.Title = "Chicken is *really* tasty!";

      } ).fail( done );

    } );

  } );

  describe( "delete", function () {

    this.timeout( 10000 );
    this.slow( 6000 );

    it( "should delete an item", function ( done ) {

      var itemService = new ItemService({
        url: "/sitecore/api/ssc/item"
      } );

      var itemId = test1Item.ItemID;

      itemService.fetchItem( itemId ).database( "master" ).execute().then( function ( test1Item ) {

        test1Item.destroy().database( "master" ).execute().then( function () {

          done();

        } ).fail( done );

      } ).fail( done );

    } );

  } );

} );