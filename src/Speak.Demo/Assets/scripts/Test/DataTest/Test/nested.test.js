describe("Testing Nested Entity Service", function () {


  // after( function ( done ) {
  //   this.timeout( 10000 );
  //   this.slow( 10000 );
  //   deleteTestItems( done );
  // } );

  it("should exist", function () {
    EntityService.should.exist;
  });

  it("should fetch items which has a created Date and a list of Object", function (done) {

    var service = new EntityService({
      url: "/sitecore/api/ssc/speak-sample/SpeakTest/"
    });

    service.fetchEntities().execute().then(function (items) {

      items.length.should.equal(2); 
      var blog = items[0];

      blog.should.exist;
      blog.Created.getTime().should.exist;

      //should have one Authors Object as array
      blog.Authors.length.should.equal(1);

      var author = blog.Authors.underlying[0];

      author.Address.should.exist;
      author.Address.Postcode.should.exist;

      done();

    }).fail(done);

  });

});