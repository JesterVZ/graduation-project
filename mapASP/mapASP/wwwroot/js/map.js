ymaps.ready(test);

var SiteMap;
var myGeoObject;
var homeCollection;
var homelist;
var connection;
var coordinates = new Array();
var ratings = new Array();


function jsonGETTER() {
	$.getJSON("https://localhost:44323/api/Rating", function (data) {
		homelist = data;
		alert("ready" + homelist.length);
		init();	
	});
}
function test() {
	jsonGETTER();
	//init();
}
function init() {
	SiteMap = new ymaps.Map('map', {
		center: [57.999878, 56.246969],
		zoom: 11
	}, {
		searchControlProvider: 'yandex#search'
	});
	for (var i = 0; i < homelist.length; i++) {
		coordinates.push(homelist[i].coords);
		ratings.push(homelist[i].rating);
	}
	console.log(coordinates[0]);
	Color(coordinates, ratings, homelist);
	SiteMap.geoObjects.add(homeCollection);


}
function Color(coords, rating, homeList) {
	var preset = new Array();

	for (var i = 0; i < rating.length; i++) {
		if (homeList[i].error != null) {
			//preset.push('islands#grayIcon');
			continue;
        }
		/*if (rating[i] < 20) {
			preset.push('islands#redIcon');

			continue;
		}
		
		if (rating[i] >= 20 && rating[i] < 50) {
			preset.push('islands#darkOrangeIcon');
			continue;
		}
		if (rating[i] >= 50 && rating[i] < 80) {
			preset.push('islands#yellowIcon');
			continue;
		}
		if (rating[i] >= 80) {
			preset.push('islands#greenIcon');
			continue;
        }*/
		var green = Math.round((rating[i] / 100) * 255);
		var red = 255 - green;
		preset.push('RGB(' + red + ',' + green + ',0)');

    }

	AddHomes(coordinates, rating, preset, homeList);
}

function AddHomes(coords, rating, preset, homelist) {
	homeCollection = new ymaps.GeoObjectCollection({}, {
		
	});
	for (var i = 0; i < coords.length; i++) {
		homeCollection.add(new ymaps.Placemark([coords[i][0], coords[i][1]], {
			balloonContent: "рейтинг: " + rating[i] + " <br> <span>Индекс задолженности дома:</span> " + homelist[i].debetorIndex + " <br> <span>Индекс счетчиков:</span> " + homelist[i].meterIndex + " <br> <span> Индекс заявок на ремонт: </span>" + homelist[i].repairIndex
		}, {
				preset: 'islands#circleIcon',
				iconColor: preset[i]
		}));
	}

}
