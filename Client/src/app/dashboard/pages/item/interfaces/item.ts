export interface Item {
    id: number;
    nameEn: string | null;
    nameAr: string | null;
    image: string | null;
    isActive: boolean | null;

    categoryId: number;
    categoryName?: string;

    cityId: number;
    cityName?: string;

    districtId: number;
    districtName?: string;

    myTypeId: number;
    myTypeName?: string;

    propertyTypeId: number;
    propertyTypeName?: string;

    statusId: number;
    statusName?: string;

    advertiseNo: number;
    adNo: number;

    latitude: number;
    longitude: number;

    soum: number;
    limit: number;
    streetWidth: number;
    space: number;
    pricePerMeter: number;

    descriptionEn: string | null;
    descriptionAr: string | null;

    hasUnits: boolean;
    rengeFrom: number;
    rangeTo: number;

    hasPassword: boolean;
    hashPassword: string | null;

    createdAt: Date;

    images: Image[];
}

export interface Image {
    id: number;
    ImageUrl: string;
}
export interface ItemFilter {
    cityId?: number | null;
    categoryId?: number | null;
    districtId?: number | null;
    propertyTypeId?: number | null;
    myTypeId?: number | null;
    statusId?: number | null;
    keyword?: string | null;
    adNo?: number | null;
    advertiseNo?: number | null;
}

export interface PaginatedResponse<T> {
    data: T[];
    totalCount: number;
    pageNumber: number;
    pageSize: number;
}

export interface PaginationDto {
    PageNumber?: number;
    pageSize?: number;
}
