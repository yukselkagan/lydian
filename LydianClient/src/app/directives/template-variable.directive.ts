import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appTemplateVariable]'
})
export class TemplateVariableDirective {

  constructor(private templateRef: TemplateRef<any>, private vcRef: ViewContainerRef) { }

  @Input()
    set appTemplateVariable(context: unknown) {
        this.context.$implicit = this.context.appTemplateVariable = context;

        if (!this.hasView) {
            this.vcRef.createEmbeddedView(this.templateRef, this.context);
            this.hasView = true;
        }
    }

    private context: {
        $implicit: unknown;
        appTemplateVariable: unknown;
    } = {
        $implicit: null,
        appTemplateVariable: null,
    };

    private hasView: boolean = false;

}
