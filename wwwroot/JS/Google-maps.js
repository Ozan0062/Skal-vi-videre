let map;
let marker;// Global variable for the marker
let geocoder;

// Initialiser kortet uden pin på brugerens geolocation
function initMap(containerId, latitude, longitude) {
    map = new google.maps.Map(document.getElementById(containerId), {
        center: { lat: latitude, lng: longitude },
        zoom: 13
    });

    // Vi vil ikke placere en pin ved brugerens oprindelige geolocation
    // Kun center kortet på brugerens oprindelige koordinater, uden at placere en pin
    map.setCenter({ lat: latitude, lng: longitude });
}

// Opdater kortets lokation, når en ny adresse er angivet
function updateMapLocation(address) {
    const geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status === 'OK') {
            const location = results[0].geometry.location;

            // Hvis en marker allerede eksisterer, fjern den
            if (marker) {
                marker.setMap(null);
            }

            // Placer en pin på den nye lokation
            marker = new google.maps.Marker({
                map: map,
                position: location,
                title: address
            });

            // Opdater kortets center til den nye lokation
            map.setCenter(location);
            map.setZoom(15);  // Zoom ind lidt tættere
        } else {
            alert('Adresse kunne ikke findes: ' + status);
        }
    });
}

// Funktion til at hente brugerens aktuelle geolocation
function getUserLocation() {
    return new Promise((resolve, reject) => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                resolve({
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                });
            }, function () {
                reject('Geolocation failed');
            });
        } else {
            reject('Geolocation is not supported by this browser.');
        }
    });
}