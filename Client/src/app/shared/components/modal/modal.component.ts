import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal',
  imports: [CommonModule],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.css'
})
export class ModalComponent {
  @Input() visible = false;
  @Input() title = 'Create';
  @Output() closed = new EventEmitter<void>();

  close() {
    this.closed.emit();
  }
}
