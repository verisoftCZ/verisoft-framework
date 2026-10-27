describe('Extract first key from errors object', () => {
    it('should return the first key from an object with multiple keys', () => {
      const errors = { name: 'Required', email: 'Invalid' };
      const key = Object.keys(errors)[0];
      expect(key).toBe('name');
    });
  
    it('should return undefined for an empty object', () => {
      const errors = {};
      const key = Object.keys(errors)[0];
      expect(key).toBeUndefined();
    });
  
    it('should return the only key in a single-key object', () => {
      const errors = { password: 'Too short' };
      const key = Object.keys(errors)[0];
      expect(key).toBe('password');
    });
  });