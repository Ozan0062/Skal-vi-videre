//// Define the initMap function globally so it's accessible from Blazor
//window.initMap = (mapContainerId, lat, lng) => {
//    const location = { lat: lat, lng: lng };
//    const map = new google.maps.Map(document.getElementById(mapContainerId), {
//        zoom: 8,
//        center: location,
//    });
//    new google.maps.Marker({
//        position: location,
//        map: map,
//    });
//};

let map, geocoder, marker;

window.initMap = (mapContainerId, lat, lng) => {
    const location = { lat: lat, lng: lng };
    const mapContainer = document.getElementById(mapContainerId);

    if (mapContainer) {
        map = new google.maps.Map(mapContainer, {
            zoom: 8,
            center: location,
        });

        marker = new google.maps.Marker({
            position: location,
            map: map,
        });

        geocoder = new google.maps.Geocoder();

        if (!geocoder) {
            console.error("Geocoder initialization failed.");
        }
    } else {
        console.error("Map container not found.");
    }
};

window.updateMapLocation = (address) => {
    const geocoder = new google.maps.Geocoder();

    return new Promise((resolve, reject) => {
        if (address) {
            geocoder.geocode({ address: address }, (results, status) => {
                if (status === "OK" && results.length > 0) {
                    const location = results[0].geometry.location;
                    resolve({ Lat: location.lat(), Lng: location.lng() });
                } else {
                    console.error("Geocode failed: " + status);
                    resolve(null); // Return null, hvis der ikke findes et resultat
                }
            });
        } else {
            resolve(null); // Return null, hvis adressen er tom
        }
    });
};