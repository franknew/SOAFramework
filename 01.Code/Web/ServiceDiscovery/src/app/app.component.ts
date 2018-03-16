import { Component, Output } from '@angular/core';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { DiscoveryService } from './services/DiscoveryService';
import { NotifyModel } from './components/ajax/NotifyModel';
import { ServiceModel } from './entities/ServiceModel';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  serviceList: ServiceModel[] = [];
  categoryList: string[] = [];
  loading: boolean = false;

  constructor(private service: DiscoveryService) {}

  ngOnInit(): void {
    this.loading = true;
    let success = new NotifyModel();
    success.callback = (data)=> {
      this.serviceList = data;
      this.categoryList.push("All");
      for (let s of this.serviceList) {
        let hasController = false;
        for (let c of this.categoryList) {
          if (c == s.category) {
            hasController = true;
            break;
          }
        }
        if (!hasController) this.categoryList.push(s.category);
      }
      this.loading = false;
    }
    let failed = new NotifyModel();
    failed.callback = (error)=> {
      this.loading = false;
    }
    this.service.getServiceList([success], [failed]);
  }

  @Output()
  get ServiceList(): ServiceModel[] {
    return this.serviceList;
  }
}
