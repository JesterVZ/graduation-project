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
	Color(coordinates, ratings);
	SiteMap.geoObjects.add(homeCollection);


}
function Color(coords, rating) {
	var preset = new Array();
	for (var i = 0; i < rating.length; i++) {
		if (rating[i] < 20) {
			preset.push('islands#redIcon');
		}
		if (rating[i] > 20 && rating[i] < 50) {
			preset.push('islands#darkOrangeIcon');
		}
		if (rating[i] > 50 && rating[i] < 80) {
			preset.push('islands#yellowIcon');
		}
		if (rating[i] > 80) {
			preset.push('islands#greenIcon');
        }

    }

	AddHomes(coordinates, rating, preset);
}
function AddHomes(coords, rating, preset) {
	homeCollection = new ymaps.GeoObjectCollection({}, {
		
	});
	for (var i = 0; i < coords.length; i++) {
		homeCollection.add(new ymaps.Placemark([coords[i][0], coords[i][1]], {
			iconCaption: rating[i]
		}, {
			preset: preset[i]
		}));
	}

}