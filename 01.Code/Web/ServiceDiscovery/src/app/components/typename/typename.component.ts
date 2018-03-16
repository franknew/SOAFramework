import { Component, Input, EventEmitter, Output } from '@angular/core';
import { TypeModel } from '../../entities/TypeModel';


@Component({
    moduleId: 'typename',
    selector: 'typename',
    templateUrl: 'typename.component.html',
})
export class TypenameComponent {

    type: TypeModel;

    @Output() OnDisplayType = new EventEmitter<TypeModel>();

    @Input()
    set Type(t: TypeModel) {
        this.type = t;
    }

    displayType(item: TypeModel) {
        this.OnDisplayType.emit(item);
    }
}
