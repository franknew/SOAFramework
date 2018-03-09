import { Component, Input, Output } from '@angular/core';

@Component({
    moduleId: 'service-category',
    selector: 'service-category',
    templateUrl: 'service-category.component.html',
})
export class ServiceCategoryComponent {
    categoryList: string[] = [];
    selectedCategory: string = "All";

    @Input()
    set CategoryList(categoryList: string[]) {
        if (categoryList != null) this.categoryList = categoryList;
        else categoryList = [];
    }

    @Output()
    get SelectedCategory(): string {
        return this.selectedCategory;
    }

    selectCategory(category: string) {
        this.selectedCategory = category;
    }
}
