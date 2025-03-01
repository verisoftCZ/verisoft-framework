import { ColumnDefinition } from '@verisoft/ui-core';

export const carColumns: ColumnDefinition<Car>[] = [
  { id: 'type' },
  { id: 'model' },
  { id: 'year', width: '10%' },
  { id: 'quantity', headerName: 'QTY', width: 100 },
  { id: 'price' },
];

export const carData: Car[] = [
  { type: 'sedan', model: 'A4', year: 2018, quantity: 5, price: 1600000 },
  {
    type: 'sports car',
    model: 'Ferrari',
    year: 2017,
    quantity: 1,
    price: 4500000,
  },
  { type: 'SUV', model: 'Tiguan', year: 2021, quantity: 3, price: 950000 },
  { type: 'hatchback', model: 'Golf', year: 2019, quantity: 8, price: 450000 },
  { type: 'pickup', model: 'Ranger', year: 2020, quantity: 2, price: 1200000 },
  {
    type: 'luxury sedan',
    model: 'S-Class',
    year: 2022,
    quantity: 1,
    price: 3100000,
  },
  {
    type: 'electric car',
    model: 'Model 3',
    year: 2023,
    quantity: 4,
    price: 1700000,
  },
  { type: 'compact', model: 'Civic', year: 2015, quantity: 6, price: 320000 },
  { type: 'crossover', model: 'CX-5', year: 2020, quantity: 3, price: 870000 },
  { type: 'roadster', model: 'MX-5', year: 2016, quantity: 2, price: 700000 },
  { type: 'convertible', model: 'Z4', year: 2019, quantity: 1, price: 2400000 },
  {
    type: 'van',
    model: 'Transporter',
    year: 2021,
    quantity: 4,
    price: 1900000,
  },
  {
    type: 'minivan',
    model: 'Odyssey',
    year: 2018,
    quantity: 3,
    price: 1200000,
  },
  { type: 'coupe', model: 'Mustang', year: 2022, quantity: 2, price: 3500000 },
  {
    type: 'off-road',
    model: 'Wrangler',
    year: 2017,
    quantity: 5,
    price: 1500000,
  },
  { type: 'hybrid', model: 'Prius', year: 2020, quantity: 7, price: 820000 },
  {
    type: 'luxury SUV',
    model: 'Range Rover',
    year: 2021,
    quantity: 1,
    price: 4200000,
  },
  { type: 'microcar', model: 'Smart', year: 2019, quantity: 6, price: 300000 },
  { type: 'wagon', model: 'V60', year: 2022, quantity: 4, price: 1400000 },
  { type: 'city car', model: 'i10', year: 2023, quantity: 10, price: 400000 },
  {
    type: 'luxury sports car',
    model: 'Lamborghini Huracan',
    year: 2020,
    quantity: 1,
    price: 6400000,
  },
  {
    type: 'executive',
    model: '5 Series',
    year: 2018,
    quantity: 3,
    price: 2000000,
  },
  {
    type: 'family car',
    model: 'Passat',
    year: 2019,
    quantity: 6,
    price: 870000,
  },
  {
    type: 'muscle car',
    model: 'Camaro',
    year: 2017,
    quantity: 2,
    price: 2800000,
  },
  {
    type: 'classic car',
    model: 'Beetle',
    year: 1965,
    quantity: 1,
    price: 1200000,
  },
  {
    type: 'track car',
    model: 'Caterham Seven',
    year: 2022,
    quantity: 1,
    price: 2100000,
  },
  {
    type: 'supercar',
    model: 'McLaren P1',
    year: 2019,
    quantity: 1,
    price: 11000000,
  },
  {
    type: 'electric SUV',
    model: 'Tesla Model X',
    year: 2023,
    quantity: 2,
    price: 2200000,
  },
  {
    type: 'classic convertible',
    model: 'Jaguar E-Type',
    year: 1969,
    quantity: 1,
    price: 3000000,
  },
  {
    type: 'cargo van',
    model: 'Sprinter',
    year: 2021,
    quantity: 5,
    price: 1800000,
  },
  {
    type: 'high-performance sedan',
    model: 'RS7',
    year: 2022,
    quantity: 2,
    price: 4500000,
  },
  {
    type: 'budget sedan',
    model: 'Accent',
    year: 2020,
    quantity: 10,
    price: 450000,
  },
  {
    type: 'luxury convertible',
    model: 'Bentley Continental',
    year: 2018,
    quantity: 1,
    price: 9500000,
  },
  {
    type: 'eco-friendly car',
    model: 'Leaf',
    year: 2021,
    quantity: 4,
    price: 600000,
  },
  {
    type: 'grand tourer',
    model: 'DB11',
    year: 2019,
    quantity: 2,
    price: 7200000,
  },
  {
    type: 'off-road SUV',
    model: 'Defender',
    year: 2022,
    quantity: 3,
    price: 3000000,
  },
  {
    type: 'performance hatchback',
    model: 'Focus RS',
    year: 2018,
    quantity: 4,
    price: 1100000,
  },
  {
    type: 'compact sedan',
    model: 'Elantra',
    year: 2019,
    quantity: 6,
    price: 580000,
  },
  {
    type: 'luxury coupe',
    model: 'C-Class Coupe',
    year: 2021,
    quantity: 2,
    price: 2600000,
  },
  {
    type: 'electric hatchback',
    model: 'i3',
    year: 2020,
    quantity: 3,
    price: 950000,
  },
  {
    type: 'premium SUV',
    model: 'XC90',
    year: 2023,
    quantity: 1,
    price: 3500000,
  },
  {
    type: 'sport compact',
    model: 'Golf GTI',
    year: 2020,
    quantity: 3,
    price: 1000000,
  },
  {
    type: 'compact SUV',
    model: 'Kona',
    year: 2021,
    quantity: 7,
    price: 850000,
  },
  {
    type: 'retro car',
    model: 'Mini Cooper',
    year: 2016,
    quantity: 5,
    price: 950000,
  },
  {
    type: 'luxury hybrid',
    model: 'Lexus RX450h',
    year: 2022,
    quantity: 2,
    price: 4000000,
  },
  {
    type: 'sports car',
    model: 'Porsche 911',
    year: 2020,
    quantity: 2,
    price: 5400000,
  },
  {
    type: 'utility vehicle',
    model: 'Duster',
    year: 2020,
    quantity: 9,
    price: 700000,
  },
  {
    type: 'mid-size SUV',
    model: 'RAV4',
    year: 2021,
    quantity: 6,
    price: 1200000,
  },
  {
    type: 'supermini',
    model: 'Fiesta',
    year: 2019,
    quantity: 8,
    price: 450000,
  },
  {
    type: 'compact MPV',
    model: 'Scenic',
    year: 2018,
    quantity: 4,
    price: 550000,
  },
  { type: 'compact', model: 'Yaris', year: 2021, quantity: 10, price: 480000 },
  {
    type: 'electric car',
    model: 'Ioniq 5',
    year: 2023,
    quantity: 5,
    price: 1600000,
  },
  {
    type: 'luxury sedan',
    model: 'A8',
    year: 2020,
    quantity: 3,
    price: 3200000,
  },
  {
    type: 'sports car',
    model: 'Corvette',
    year: 2019,
    quantity: 2,
    price: 3200000,
  },
  { type: 'minivan', model: 'Sienna', year: 2021, quantity: 4, price: 1500000 },
  { type: 'coupe', model: '4 Series', year: 2022, quantity: 2, price: 1800000 },
  { type: 'luxury SUV', model: 'X5', year: 2023, quantity: 1, price: 2800000 },
  { type: 'crossover', model: 'Kuga', year: 2020, quantity: 6, price: 980000 },
  {
    type: 'city car',
    model: 'Picanto',
    year: 2022,
    quantity: 8,
    price: 350000,
  },
  { type: 'hatchback', model: 'Clio', year: 2018, quantity: 5, price: 460000 },
  {
    type: 'electric SUV',
    model: 'Mustang Mach-E',
    year: 2021,
    quantity: 2,
    price: 1800000,
  },
  { type: 'sedan', model: 'Mazda 6', year: 2019, quantity: 4, price: 750000 },
  { type: 'roadster', model: 'S2000', year: 2009, quantity: 1, price: 1300000 },
  {
    type: 'luxury convertible',
    model: 'E-Class Cabriolet',
    year: 2021,
    quantity: 2,
    price: 2600000,
  },
  {
    type: 'muscle car',
    model: 'Challenger',
    year: 2020,
    quantity: 3,
    price: 2200000,
  },
  { type: 'van', model: 'Caddy', year: 2022, quantity: 7, price: 1200000 },
  {
    type: 'sports car',
    model: 'GT-R',
    year: 2019,
    quantity: 1,
    price: 3700000,
  },
  { type: 'hybrid', model: 'Insight', year: 2020, quantity: 3, price: 950000 },
  {
    type: 'luxury coupe',
    model: 'LC 500',
    year: 2023,
    quantity: 1,
    price: 3700000,
  },
  {
    type: 'compact SUV',
    model: 'Sportage',
    year: 2020,
    quantity: 6,
    price: 860000,
  },
  { type: 'wagon', model: 'Octavia', year: 2018, quantity: 9, price: 740000 },
  {
    type: 'supercar',
    model: 'Aventador',
    year: 2021,
    quantity: 1,
    price: 9000000,
  },
  { type: 'sedan', model: 'Camry', year: 2019, quantity: 7, price: 860000 },
  { type: 'retro car', model: '500', year: 2020, quantity: 4, price: 400000 },
  {
    type: 'electric hatchback',
    model: 'Zoe',
    year: 2021,
    quantity: 5,
    price: 700000,
  },
  { type: 'luxury SUV', model: 'Q7', year: 2022, quantity: 2, price: 3100000 },
  {
    type: 'family car',
    model: 'Insignia',
    year: 2018,
    quantity: 8,
    price: 720000,
  },
  {
    type: 'electric car',
    model: 'Bolt',
    year: 2023,
    quantity: 3,
    price: 1200000,
  },
  {
    type: 'classic sports car',
    model: '911 Carrera',
    year: 1998,
    quantity: 1,
    price: 2100000,
  },
  { type: 'microcar', model: 'Up!', year: 2021, quantity: 10, price: 420000 },
  { type: 'off-road', model: 'Jimny', year: 2020, quantity: 4, price: 950000 },
  { type: 'supermini', model: 'Corsa', year: 2019, quantity: 7, price: 430000 },
  {
    type: 'compact sedan',
    model: 'Altima',
    year: 2021,
    quantity: 5,
    price: 870000,
  },
  {
    type: 'cargo van',
    model: 'NV200',
    year: 2022,
    quantity: 6,
    price: 1400000,
  },
  {
    type: 'performance coupe',
    model: 'Supra',
    year: 2020,
    quantity: 2,
    price: 2400000,
  },
  {
    type: 'eco-friendly car',
    model: 'Mirai',
    year: 2022,
    quantity: 3,
    price: 1900000,
  },
  {
    type: 'luxury hybrid',
    model: 'RX 500h',
    year: 2023,
    quantity: 1,
    price: 3700000,
  },
  {
    type: 'high-performance sedan',
    model: 'CT5-V',
    year: 2021,
    quantity: 3,
    price: 2800000,
  },
  {
    type: 'classic convertible',
    model: 'MG MGB',
    year: 1972,
    quantity: 1,
    price: 800000,
  },
  {
    type: 'grand tourer',
    model: 'Vantage',
    year: 2019,
    quantity: 2,
    price: 5500000,
  },
  {
    type: 'compact crossover',
    model: 'HR-V',
    year: 2022,
    quantity: 6,
    price: 780000,
  },
  {
    type: 'electric sports car',
    model: 'Taycan',
    year: 2021,
    quantity: 2,
    price: 3500000,
  },
  {
    type: 'track car',
    model: 'Ariel Atom',
    year: 2020,
    quantity: 1,
    price: 2500000,
  },
  {
    type: 'premium SUV',
    model: 'GLE',
    year: 2023,
    quantity: 2,
    price: 2900000,
  },
  {
    type: 'family van',
    model: 'Galaxy',
    year: 2020,
    quantity: 4,
    price: 1600000,
  },
  {
    type: 'compact luxury SUV',
    model: 'XC60',
    year: 2021,
    quantity: 3,
    price: 2100000,
  },
  {
    type: 'entry-level sedan',
    model: 'Sentra',
    year: 2019,
    quantity: 8,
    price: 640000,
  },
  {
    type: 'electric city car',
    model: 'e-Up!',
    year: 2022,
    quantity: 5,
    price: 580000,
  },
];

export interface Car {
  type: string;
  model: string;
  year: number;
  quantity: number;
  price: number;
}