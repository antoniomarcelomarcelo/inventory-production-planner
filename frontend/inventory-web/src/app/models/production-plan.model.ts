export interface ProductionPlanItem {
  productId: number;
  productCode: string;
  productName: string;
  unitPrice: number;
  producibleQuantity: number;
  totalValue: number;
  bottleneckRawMaterial?: string | null;
}

export interface ProductionPlanResult {
  items: ProductionPlanItem[];
  grandTotalValue: number;
}
