export type Role = "Admin" | "User";

export type UserDto = {
    id: string;
    email: string;
    role: Role;
    firstName?: string;
    lastName?: string;
};

export type StudyDto = {
    id: number;
    userId: string;
    title: string;
    institution: string;
    startDate: string;
    endDate?: string;
};

export type AddressDto = {
    id: number;
    userId: string;
    street: string;
    city: string;
    state?: string;
    country: string;
    zipCode: string;
};
