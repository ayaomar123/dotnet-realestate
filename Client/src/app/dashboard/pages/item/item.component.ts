import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { General } from '../../../shared/interfaces/General';
import { NotificationService } from '../../../shared/services/notification.service';
import { Item, PaginatedResponse } from './interfaces/item';
import { ItemService } from './services/item.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-item',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './item.component.html',
  styleUrl: './item.component.css'
})
export class ItemComponent implements OnInit {
  private readonly service = inject(ItemService);
  private readonly fb = inject(FormBuilder);
  private readonly toastr = inject(NotificationService);

  data: Item[] = [];
  title: string = 'Categories';

  notifications: { type: string; message: string }[] = [];
  private notificationSubscription!: Subscription;

  ngOnInit(): void {
    this.loadData();
    this.subscribeToNotifications();
  }

  private subscribeToNotifications(): void {
    this.notificationSubscription = this.toastr.notifications$.subscribe(notification => {
      this.notifications.push(notification);
      setTimeout(() => this.notifications.shift(), 3000);
    });
  }

  private loadData(): void {
    this.service.get().subscribe((res: General<PaginatedResponse<Item>>) => {
      this.data = res.data.data;
    });
  }

  getImageUrl(image?: string | null): string {
    return image ? `${environment.assetsUrl}/${image}` : '';
  }



  changeStatus(Item: Item): void {
    this.service.updateStatus(Item.id).subscribe({
      next: () => {
        this.loadData();
        this.toastr.success('Item status updated successfully!');
      },
      error: err => this.toastr.error('Data not saved' + err)
    });
  }

  deleteItem(id: number): void {
    if (confirm('Are you sure you want to delete this Item?')) {
      this.service.delete(id).subscribe({
        next: () => {
          this.loadData();
          this.toastr.success('Item deleted successfully!');
        },
        error: err => {
          console.error('Error deleting Item:', err);
          this.toastr.error('Data not saved' + err);
        }
      });
    }
  }

}
