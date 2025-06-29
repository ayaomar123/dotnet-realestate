import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ItemService } from '../services/item.service';
import { NotificationService } from '../../../../shared/services/notification.service';

@Component({
  selector: 'app-create-item',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-item.component.html',
  styleUrl: './create-item.component.css'
})
export class CreateItemComponent {
  private readonly service = inject(ItemService);
  private readonly fb = inject(FormBuilder);
  private readonly toastr = inject(NotificationService);

  form!: FormGroup;
  onSubmit() {
    throw new Error('Method not implemented.');
  }

}
