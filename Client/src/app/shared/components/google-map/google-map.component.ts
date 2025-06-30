import { AfterViewInit, Component, ElementRef, EventEmitter, Output, ViewChild } from '@angular/core';
declare const google: any;

@Component({
  selector: 'app-google-map',
  imports: [],
  templateUrl: './google-map.component.html',
  styleUrl: './google-map.component.css'
})

export class GoogleMapComponent implements AfterViewInit {
  @ViewChild('mapContainer', { static: false }) mapElementRef!: ElementRef;

  @Output() coordinatesSelected = new EventEmitter<{ lat: number; lng: number }>();

  ngAfterViewInit(): void {
    const mapEl = this.mapElementRef.nativeElement;
    const defaultCoords = { lat: 24.7136, lng: 46.6753 };
    const map = new google.maps.Map(mapEl, {
      center: defaultCoords,
      zoom: 6
    });

    let marker = new google.maps.Marker({
      position: defaultCoords,
      map,
      draggable: true
    });

    map.addListener('click', (e: google.maps.MapMouseEvent) => {
      const position = e.latLng!;
      marker.setPosition(position);
      this.coordinatesSelected.emit({ lat: position.lat(), lng: position.lng() });
    });

    marker.addListener('dragend', () => {
      const position = marker.getPosition()!;
      this.coordinatesSelected.emit({ lat: position.lat(), lng: position.lng() });
    });
  }
}
