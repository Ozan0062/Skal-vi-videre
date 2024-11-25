// Define the initMap function globally so it's accessible from Blazor
window.initMap = (mapContainerId, lat, lng) => {
    const location = { lat: lat, lng: lng };
    const map = new google.maps.Map(document.getElementById(mapContainerId), {
        zoom: 8,
        center: location,
    });
    new google.maps.Marker({
        position: location,
        map: map,
    });
};
