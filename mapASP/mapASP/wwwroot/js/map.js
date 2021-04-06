ymaps.ready(init);

var SiteMap;
var myGeoObject;
var homeCollection;
var homelist;
var connection;
var coordinates = new Array();
var ratings = new Array();

$.getJSON("https://localhost:44391/api/Rating", function (data) {
	homelist = data;
});

function init() {
	SiteMap = new ymaps.Map('map', {
		center: [55.674, 37.601],
		zoom: 11
	}, {
		searchControlProvider: 'yandex#search'
	});
	for (var i = 0; i < homelist.length; i++) {
		coordinates.push(homelist[i].coordinates);
		ratings.push(homelist[i].rating);
	}
	console.log(coordinates[0]);
	AddHomes(coordinates, ratings);
	SiteMap.geoObjects.add(homeCollection);


}
function AddHomes(coords, rating) {
	homeCollection = new ymaps.GeoObjectCollection({}, {
		preset: 'islands#redDotIconWithCaption'
	});
	for (var i = 0; i < coords.length; i++) {
		homeCollection.add(new ymaps.Placemark([coords[i][0], coords[i][1]], {
			balloonContent: '',
			iconCaption: rating[i]
		}));
	}

}