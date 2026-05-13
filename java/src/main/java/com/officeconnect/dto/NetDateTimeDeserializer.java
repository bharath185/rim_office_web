package com.officeconnect.dto;

import com.fasterxml.jackson.core.JsonParser;
import com.fasterxml.jackson.databind.DeserializationContext;
import com.fasterxml.jackson.databind.deser.std.StdDeserializer;
import java.io.IOException;
import java.util.Date;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class NetDateTimeDeserializer extends StdDeserializer<Date> {

    private static final Pattern DATE_PATTERN = Pattern.compile("/Date\\((\\d+)\\)/");

    public NetDateTimeDeserializer() {
        super(Date.class);
    }

    @Override
    public Date deserialize(JsonParser p, DeserializationContext ctxt) throws IOException {
        String value = p.readValueAs(String.class);
        if (value == null || value.trim().isEmpty()) {
            return null;
        }
        Matcher matcher = DATE_PATTERN.matcher(value.trim());
        if (matcher.matches()) {
            long timestamp = Long.parseLong(matcher.group(1));
            return new Date(timestamp);
        }
        try {
            return new Date(value);
        } catch (Exception e) {
            return null;
        }
    }
}
