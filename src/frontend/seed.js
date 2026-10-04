// Script to seed test data into the Campus database
// To execute, open a terminal in the src/Frontend directory and run: node seed.js

const API_URL = 'http://localhost:5000/api/campus';

async function postData(endpoint, data) {
  try {
    const response = await fetch(`${API_URL}${endpoint}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Error at ${endpoint}: ${response.status} - ${errorText}`);
    }

    const result = await response.json();
    return result.id; // Assume the backend returns the object with its Id
  } catch (err) {
    console.error(err.message);
    return null;
  }
}

async function runSeed() {
  console.log('Initializing test data injection...');

  // 1. Create a Campus
  console.log('Creating Campus...');
  const campusId = await postData('/campuses', {
    name: 'Campus San Francisco',
    address: 'Manuel Montt 056, Temuco',
    latitude: 50, // Relative % position on the map
    longitude: 50
  });

  if (!campusId) return console.error('Campus creation failed.');

  // 2. Create a Room Category (e.g. Classrooms, Laboratory)
  console.log('Creating Category...');
  const categoryId = await postData('/categories', {
    name: 'Sala de Clases',
    icon: 'book',
    description: 'Sala de uso general para clases teóricas'
  });

  if (!categoryId) return console.error('Category creation failed.');

  // 3. Create Buildings
  console.log('Creating Buildings...');
  const buildings = [
    { name: 'Edificio T', floorsCount: 4, latitude: 35, longitude: 45 },
    { name: 'Biblioteca Central', floorsCount: 2, latitude: 60, longitude: 70 },
    { name: 'Edificio C', floorsCount: 3, latitude: 20, longitude: 80 }
  ];

  for (const b of buildings) {
    const buildingId = await postData('/buildings', {
      campusId: campusId,
      name: b.name,
      floorsCount: b.floorsCount,
      latitude: b.latitude,
      longitude: b.longitude
    });

    if (buildingId) {
      // 4. Create Rooms for this Building
      console.log(`Creating rooms for ${b.name}...`);
      for (let i = 1; i <= 3; i++) {
        await postData('/rooms', {
          buildingId: buildingId,
          categoryId: categoryId,
          name: `Sala ${b.name[b.name.length - 1]}-${i}01`,
          floor: i,
          number: `${i}01`
        });
      }
    }
  }

  console.log('Data injection completed successfully.');
  console.log('Reload the map application to see the seeded data.');
}

runSeed();
