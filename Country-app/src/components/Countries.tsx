import { Box, CircularProgress, Container, Grid, Paper, Stack, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { Api_base_url } from './Endpoint';

interface Country {
  cca3: string;
  name: {
    common: string;
  };
  capital?: string[];
  region?: string;
  population?: number;
  flags?: {
    png: string;
  };
}

const Countries: React.FC = () => {
  const [countries, setCountries] = useState<Country[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedCountry, setSelectedCountry] = useState<Country | null>(null);

  useEffect(() => {
    const fetchCountries = async () => {
      try {
        const url = Api_base_url + 'Countries';
               console.log(url);
              const response = await fetch(url, {
                method: 'GET',
                headers: {
                  'Content-Type': 'application/json',
                  'Access-Control-Allow-Origin': '*'
                }
              });
        console.log(response);
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        const data = await response.json();
        setCountries(data);
        setLoading(false);
      } catch (error: any) {
        setError(error.message);
        setLoading(false);
      }
    };

    fetchCountries();
  }, []);

  if (loading) {
    return <CircularProgress />;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  function handleClicked(country: Country) {
    setSelectedCountry(country);
  }

  return (
    <Container>
      <Typography variant="h4" gutterBottom align="center">
        Country List
      </Typography>

      <Typography variant="h4" gutterBottom align="center">
        Total Countries: {countries.length}
      </Typography>
      <Stack direction={'row'} spacing={2} sx={{ mb: 2 }}>
        <Grid container spacing={3} sx={{ flex: 1, maxHeight: '50vh', overflow: 'auto' }}>
          {countries.map((country, index) => (
            <Grid item xs={12} sm={6} md={4} lg={3} key={index}>
              <Container>
                <Paper elevation={3} onClick={() => handleClicked(country)} style={{ padding: 16, cursor: 'pointer' }}>
                  <Typography variant="body1">{country.name.common}</Typography>
                  <img src={country.flags?.png} alt={`${country.name.common} flag`} style={{ width: '100px', height: 'auto' }} />
                </Paper>
              </Container>
            </Grid>
          ))}
        </Grid>
        {selectedCountry && (
          <Box component={Paper} elevation={3} style={{ padding: 16, marginTop: 16 }}>
            <Typography variant="body1"><strong>Country: </strong>{selectedCountry.name.common}</Typography>
            <Typography variant="body1"><strong>Capital:</strong> {selectedCountry.capital?.join(', ')}</Typography>
            <Typography variant="body1"><strong>Region:</strong> {selectedCountry.region}</Typography>
            <Typography variant="body1"><strong>Population:</strong> {selectedCountry.population?.toLocaleString()}</Typography>
          </Box>
        )}
      </Stack>
    </Container>
  );
};

export default Countries;