import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { AjaxModule } from './components/ajax/ajax.module';
import { NgZorroAntdModule, NzFormItemDirective } from 'ng-zorro-antd';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ServiceCategoryComponent } from './components/service-category/service-category.component';
import { ServiceListComponent } from './components/service-list/service-list.component';
import { DiscoveryService } from './services/DiscoveryService';
import { HttpClientModule } from '@angular/common/http';
import { ServiceModel } from './entities/ServiceModel';
import { TypeModel } from './entities/TypeModel';
import { ArgModel } from './entities/ArgModel';
import { TypenameComponent } from './components/typename/typename.component';

@NgModule({
  declarations: [
    AppComponent,
    ServiceCategoryComponent,
    ServiceListComponent,
    ServiceModel,
    TypeModel,
    ArgModel,
    DiscoveryService,
    TypenameComponent,
      ],
  imports: [
    BrowserModule,
    FormsModule,
    BrowserAnimationsModule,
    AjaxModule,
    NgZorroAntdModule.forRoot(),
    HttpClientModule,
  ],
  providers: [ DiscoveryService ],
  bootstrap: [AppComponent]
})
export class AppModule { }
