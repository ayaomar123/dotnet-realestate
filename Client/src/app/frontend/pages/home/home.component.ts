import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Item, PaginatedResponse } from '../../../dashboard/pages/item/interfaces/item';
import { ItemService } from '../../../dashboard/pages/item/services/item.service';
import { General } from '../../../shared/interfaces/General';
import { environment } from '../../../../environments/environment';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [CommonModule,RouterLink],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  isSearchModalOpen = false;
  private readonly service = inject(ItemService);
  data: Item[] = [];

  ngOnInit(): void {
    this.loadData();
  }

  private loadData(): void {

    this.service.get().subscribe({
      next: (res: General<PaginatedResponse<Item>>) => {
        this.data = res.data.data;
        console.log(this.data);
      },
      error: (err) => {
        console.error('Error:', err);
      }
    });
  }

  getImageUrl(image?: string | null): string {
    return image ? `${environment.assetsUrl}/${image}` : '';
  }
}
