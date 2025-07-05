import { Component, inject } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Item, PaginatedResponse } from '../../../dashboard/pages/item/interfaces/item';
import { ItemService } from '../../../dashboard/pages/item/services/item.service';
import { General } from '../../../shared/interfaces/General';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-details',
  imports: [CommonModule],
  templateUrl: './details.component.html',
  styleUrl: './details.component.css'
})
export class DetailsComponent {
  private readonly service = inject(ItemService);
  private readonly route = inject(ActivatedRoute);
  id!: number;
  item: Item | null = null;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadData(this.id);
  }

  private loadData(id: number): void {
    this.service.getById(id).subscribe({
      next: (res: General<Item>) => {
        this.item = res.data;
        console.log(this.item);
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
